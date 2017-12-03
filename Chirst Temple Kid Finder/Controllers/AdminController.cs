#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Chirst_Temple_Kid_Finder.Models;
using PagedList;
using System.Data.Entity;
#endregion Includes

namespace Chirst_Temple_Kid_Finder.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        // Controllers
        // GET: /Admin/
        [Authorize(Roles = "Administrator")]
        #region public ActionResult Index(string searchStringUserNameOrEmail)
        public ActionResult Index(string searchStringUserNameOrEmail, string currentFilter, int? page)
        {
            try
            {
                int intPage = 1;
                int intPageSize = 5;
                int intTotalPageCount = 0;
                if (searchStringUserNameOrEmail != null)
                {
                    intPage = 1;
                }
                else
                {
                    if (currentFilter != null)
                    {
                        searchStringUserNameOrEmail = currentFilter;
                        intPage = page ?? 1;
                    }
                    else
                    {
                        searchStringUserNameOrEmail = "";
                        intPage = page ?? 1;
                    }
                }
                ViewBag.CurrentFilter = searchStringUserNameOrEmail;
                List<ExpandedUserDTO> col_UserDTO = new List<ExpandedUserDTO>();
                int intSkip = (intPage - 1) * intPageSize;

                intTotalPageCount = UserManager.Users
                    .Where(x => x.UserName.Contains(searchStringUserNameOrEmail))
                    .Count();

                var result = UserManager.Users
                    .Where(x => x.UserName.Contains(searchStringUserNameOrEmail))
                    .OrderBy(x => x.UserName)
                    .Skip(intSkip)
                    .Take(intPageSize)
                    .ToList();

                foreach (var item in result)
                {
                    ExpandedUserDTO objUserDTO = new ExpandedUserDTO();
                    objUserDTO.UserName = item.UserName;
                    objUserDTO.Email = item.Email;
                    objUserDTO.LockoutEndDateUtc = item.LockoutEndDateUtc;
                    col_UserDTO.Add(objUserDTO);
                }

                // Set the number of pages
                var _UserDTOAsIPagedList = new StaticPagedList<ExpandedUserDTO>(col_UserDTO, intPage, intPageSize, intTotalPageCount);
                
                return View(_UserDTOAsIPagedList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                List<ExpandedUserDTO> col_UserDTO = new List<ExpandedUserDTO>();
                return View(col_UserDTO.ToPagedList(1, 25));
            }
        }
        #endregion



        // Roles *****************************
        // GET: /Admin/ViewAllRoles
        [Authorize(Roles = "Administrator")]
        #region public ActionResult ViewAllRoles()
        public ActionResult ViewAllRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            List<RoleDTO> colRoleDTO = (from objRole in roleManager.Roles select new RoleDTO { ID = objRole.Id, RoleName = objRole.Name }).ToList();
            return View(colRoleDTO);
        }

        #endregion

        // GET: /Admin/AddRole

        [Authorize(Roles = "Administrator")]
        #region public ActionResult AddRole()
        public ActionResult AddRole()
        {
            RoleDTO objRoleDTO = new RoleDTO();

            return View(objRoleDTO);
        }

        #endregion

        // PUT: /Admin/AddRole
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        #region public ActionResult AddRole(RoleDTO paramRoleDTO)
        public ActionResult AddRole(RoleDTO paramRoleDTO)
        {
            try
            {
                if (paramRoleDTO == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var RoleName = paramRoleDTO.RoleName.Trim();
                if (RoleName == "")
                {
                    throw new Exception("No RoleName");
                }

                // Create Role
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

                if (!roleManager.RoleExists(RoleName))
                {
                    roleManager.Create(new IdentityRole(RoleName));
                }
                return Redirect("~/Admin/ViewAllRoles");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("AddRole");
            }
        }
        #endregion

        // DELETE: /Admin/DeleteUserRole?RoleName=TestRole
        [Authorize(Roles = "Administrator")]
        #region public ActionResult DeleteUserRole(string RoleName)
        public ActionResult DeleteUserRole(string RoleName)
        {
            try
            {
                if (RoleName == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (RoleName.ToLower() == "administrator")
                {
                    throw new Exception(String.Format("Cannot delete {0} Role.", RoleName));
                }

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

                var UsersInRole = roleManager.FindByName(RoleName).Users.Count();

                if (UsersInRole > 0)
                {
                    throw new Exception(String.Format("Canot delete {0} Role because it still has users.", RoleName));
                }

                var objRoleToDelete = (from objRole in roleManager.Roles where objRole.Name == RoleName select objRole).FirstOrDefault();

                if (objRoleToDelete != null)
                {
                    roleManager.Delete(objRoleToDelete);
                }

                else
                {
                    throw new Exception(String.Format("Cannot delete {0} Role does not exist.", RoleName));
                }

                List<RoleDTO> colRoleDTO = (from objRole in roleManager.Roles select new RoleDTO { ID = objRole.Id, RoleName = objRole.Name }).ToList();

                return View("ViewAllRoles", colRoleDTO);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                List<RoleDTO> colRoleDTO = (from objRole in roleManager.Roles select new RoleDTO { ID = objRole.Id, RoleName = objRole.Name }).ToList();
                return View("ViewAllRoles", colRoleDTO);
            }
        }
        #endregion

        // Users *****************************

        dbcaa9cff9bf624b1ebcf9a8120126a40eEntities3 db = new dbcaa9cff9bf624b1ebcf9a8120126a40eEntities3();
        List<CodeAssignTable> needsUpdate = new List<CodeAssignTable>();


        // GET: /Admin/Edit/Create 

        [Authorize(Roles = "Administrator")]

        #region public ActionResult Create()

        public ActionResult Create()

        {

            ExpandedUserDTO objExpandedUserDTO = new ExpandedUserDTO();

            ViewBag.Roles = GetAllRolesAsSelectList();

            return View(objExpandedUserDTO);

        }

        #endregion

        // PUT: /Admin/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        #region public ActionResult Create(ExpandedUserDTO paramExpandedUserDTO)
        public ActionResult Create(ExpandedUserDTO paramExpandedUserDTO)
        {
            try
            {
                if (paramExpandedUserDTO == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var Email = paramExpandedUserDTO.Email.Trim();
                var UserName = paramExpandedUserDTO.Email.Trim();
                var Password = paramExpandedUserDTO.Password.Trim();
                if (Email == "")
                {
                    throw new Exception("No Email");
                }
                if (Password == "")
                {
                    throw new Exception("No Password");
                }
                // UserName is LowerCase of the Email
                UserName = Email.ToLower();

                // Create user
                var objNewAdminUser = new ApplicationUser { UserName = UserName, Email = Email };
                var AdminUserCreateResult = UserManager.Create(objNewAdminUser, Password);
                if (AdminUserCreateResult.Succeeded == true)
                {
                    string strNewRole = Convert.ToString(Request.Form["Roles"]);
                    if (strNewRole != "0")
                    {
                        // Put user in role
                        UserManager.AddToRole(objNewAdminUser.Id, strNewRole);
                    }
                    if (strNewRole == "Parent")
                    {
                        bool emailExists = db.CodeAssignTables.AsNoTracking().Any(e => e.Email.Equals(Email));
                        List<CodeAssignTable> codeList = db.CodeAssignTables.AsNoTracking().ToList();

                        int id = db.CodeAssignTables.Count();
                        string gen = Generate();
                        CodeAssignTable entryAdd = new CodeAssignTable
                        {
                            Id = id,
                            Email = Email,
                            ChildCode = gen
                        };
                        CodeTable codeEntryAdd = new CodeTable
                        {
                            Id = id,
                            ChildCode = gen
                        };
                        db.CodeAssignTables.Add(entryAdd);
                        db.CodeTables.Add(codeEntryAdd);
                        db.SaveChanges();
                    }
                return Redirect("~/Admin");
                }
                else
                {
                    ViewBag.Roles = GetAllRolesAsSelectList();
                    ModelState.AddModelError(string.Empty,
                        "Error: Failed to create the user. Check password requirements.");
                    return View(paramExpandedUserDTO);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Roles = GetAllRolesAsSelectList();
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("Create");
            }
        }
        #endregion


        //TODO: Figure out how to change data in codeassigntable when user email is edited

        // GET: /Admin/Edit/TestUser 
        [Authorize(Roles = "Administrator")]
        #region public ActionResult EditUser(string UserName)
        public ActionResult EditUser(string UserName)
        {
            if (UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpandedUserDTO objExpandedUserDTO = GetUser(UserName);
            if (User.IsInRole("Parent"))
            {
                CodeAssignTable objCodeAssign = db.CodeAssignTables.First(x => x.Email == objExpandedUserDTO.Email);
                addCodeToList(objCodeAssign);
            }
            if (objExpandedUserDTO == null)
            {
                return HttpNotFound();
            }
            return View(objExpandedUserDTO);
        }
        #endregion

        // PUT: /Admin/EditUser
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        #region public ActionResult EditUser(ExpandedUserDTO paramExpandedUserDTO)
        public ActionResult EditUser(ExpandedUserDTO paramExpandedUserDTO)
        {
            try
            {
                if (paramExpandedUserDTO == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ExpandedUserDTO objExpandedUserDTO = UpdateDTOUser(paramExpandedUserDTO);
                CodeAssignTable objCodeAssign = pullFromList();
                if(objCodeAssign != null)
                {
                    objCodeAssign.Email = paramExpandedUserDTO.Email;
                    db.Entry(objCodeAssign).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                if (objExpandedUserDTO == null)
                {
                    return HttpNotFound();
                }
                return Redirect("~/Admin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("EditUser", GetUser(paramExpandedUserDTO.UserName));
            }
        }
        #endregion

        /*
        // GET: /Admin/AssignCode 
        [Authorize(Roles = "Administrator")]
        #region public ActionResult CodeAssignEdit(string email)
        public ActionResult CodeAssignEdit(string email)
        {
            if (email == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodeAssignTable objCodeATable = GetCodeAssignUser(email);
            if (objCodeATable == null)
            {
                return HttpNotFound();
            }
            return View(objCodeATable);
        }
        #endregion

        
        // PUT: /Admin/AssignCode
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        #region public ActionResult CodeAssignEdit(CodeAssignTable paramCodeAssignTable)
        public ActionResult CodeAssignEdit([Bind(Include = "Email, ChildCode")] CodeAssignTable paramCodeAssignTable)
        {
            try
            {
                if (paramCodeAssignTable == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                bool emailExists = db.CodeAssignTables.AsNoTracking().Any(email => email.Email.Equals(paramCodeAssignTable.Email));
                List<CodeAssignTable> codeList = db.CodeAssignTables.AsNoTracking().ToList();

                if (emailExists)
                {
                    CodeAssignTable entryUpdate = db.CodeAssignTables.FirstOrDefault(x => x.Email == paramCodeAssignTable.Email);
                    entryUpdate.ChildCode = paramCodeAssignTable.ChildCode;
                    db.Entry(entryUpdate).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                int id = db.CodeAssignTables.Count();
                CodeAssignTable entryAdd = new CodeAssignTable
                {
                    Id = id,
                    Email = paramCodeAssignTable.Email,
                    ChildCode = paramCodeAssignTable.ChildCode
                };
                CodeTable codeEntryAdd = new CodeTable
                {
                    Id = id,
                    ChildCode = paramCodeAssignTable.ChildCode
                };
                db.CodeAssignTables.Add(entryAdd);
                db.CodeTables.Add(codeEntryAdd);
                db.SaveChanges();
            }
            return Redirect("~/Admin");
        }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("CodeAssignEdit", GetCodeAssignUser(paramCodeAssignTable.Email));
            }
        }
        #endregion


        public ActionResult CodeAssignEdit(string email)
        {
            try
            {
                
                bool emailExists = db.CodeAssignTables.AsNoTracking().Any(e => e.Email.Equals(email));
                List<CodeAssignTable> codeList = db.CodeAssignTables.AsNoTracking().ToList();

                if (emailExists)
                {
                    CodeAssignTable entryUpdate = db.CodeAssignTables.FirstOrDefault(x => x.Email == email);
                    if (entryUpdate.ChildCode == null)
                    {
                        entryUpdate.ChildCode = Generate();
                        db.Entry(entryUpdate).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else throw new Exception("Code already assigned");
                }
                else
                {
                    int id = db.CodeAssignTables.Count();
                    string gen = Generate();
                    CodeAssignTable entryAdd = new CodeAssignTable
                    {
                        Id = id,
                        Email = email,
                        ChildCode = gen
                    };
                    CodeTable codeEntryAdd = new CodeTable
                    {
                        Id = id,
                        ChildCode = gen
                    };
                    db.CodeAssignTables.Add(entryAdd);
                    db.CodeTables.Add(codeEntryAdd);
                    db.SaveChanges();
                }
                return Redirect("~/Admin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("Index", GetCodeAssignUser(email));
            }
        }
        */
        // DELETE: /Admin/DeleteUser
        [Authorize(Roles = "Administrator")]
        #region public ActionResult DeleteUser(string UserName)
        public ActionResult DeleteUser(string UserName)
        {
            try
            {
                if (UserName == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (UserName.ToLower() == this.User.Identity.Name.ToLower())
                {
                    ModelState.AddModelError(string.Empty, "Error: Cannot delete the current user");
                    return View("EditUser");
                }
                ExpandedUserDTO objExpandedUserDTO = GetUser(UserName);
                if (objExpandedUserDTO == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    DeleteUser(objExpandedUserDTO);
                }
                return Redirect("~/Admin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("EditUser", GetUser(UserName));
            }
        }
        #endregion

        // GET: /Admin/EditRoles/TestUser 
        [Authorize(Roles = "Administrator")]
        #region ActionResult EditRoles(string UserName)
        public ActionResult EditRoles(string UserName)
        {
            if (UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserName = UserName.ToLower();
            // Check that we have an actual user
            ExpandedUserDTO objExpandedUserDTO = GetUser(UserName);
            if (objExpandedUserDTO == null)
            {
                return HttpNotFound();
            }
            UserAndRolesDTO objUserAndRolesDTO =
                GetUserAndRoles(UserName);
            return View(objUserAndRolesDTO);
        }
        #endregion

        // PUT: /Admin/EditRoles/TestUser 
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        #region public ActionResult EditRoles(UserAndRolesDTO paramUserAndRolesDTO)
        public ActionResult EditRoles(UserAndRolesDTO paramUserAndRolesDTO)
        {
            try
            {
                if (paramUserAndRolesDTO == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                string UserName = paramUserAndRolesDTO.UserName;
                string strNewRole = Convert.ToString(Request.Form["AddRole"]);
                if (strNewRole != "No Roles Found")
                {
                    // Go get the User
                    ApplicationUser user = UserManager.FindByName(UserName);
                    // Put user in role
                    UserManager.AddToRole(user.Id, strNewRole);
                }
                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));
                UserAndRolesDTO objUserAndRolesDTO =
                    GetUserAndRoles(UserName);
                return View(objUserAndRolesDTO);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("EditRoles");
            }
        }
        #endregion

        // DELETE: /Admin/DeleteRole?UserName="TestUser&RoleName=Administrator
        [Authorize(Roles = "Administrator")]
        #region public ActionResult DeleteRole(string UserName, string RoleName)
        public ActionResult DeleteRole(string UserName, string RoleName)
        {
            try
            {
                if ((UserName == null) || (RoleName == null))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UserName = UserName.ToLower();
                // Check that we have an actual user
                ExpandedUserDTO objExpandedUserDTO = GetUser(UserName);
                if (objExpandedUserDTO == null)
                {
                    return HttpNotFound();
                }
                if (UserName.ToLower() ==
                    this.User.Identity.Name.ToLower() && RoleName == "Administrator")
                {
                    ModelState.AddModelError(string.Empty,
                        "Error: Cannot delete Administrator Role for the current user");
                }
                // Go get the User
                ApplicationUser user = UserManager.FindByName(UserName);
                // Remove User from role
                UserManager.RemoveFromRoles(user.Id, RoleName);
                UserManager.Update(user);
                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));
                return RedirectToAction("EditRoles", new { UserName = UserName });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));
                UserAndRolesDTO objUserAndRolesDTO =
                    GetUserAndRoles(UserName);
                return View("EditRoles", objUserAndRolesDTO);
            }
        }
        #endregion

        // Utility
        #region public ApplicationUserManager UserManager
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        #endregion

        #region public ApplicationRoleManager RoleManager
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        #region private List GetAllRolesAsSelectList()
        private List<SelectListItem> GetAllRolesAsSelectList()
        {
            List<SelectListItem> SelectRoleListItems = new List<SelectListItem>();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var colRoleSelectList = roleManager.Roles.OrderBy(x => x.Name).ToList();
            SelectRoleListItems.Add(new SelectListItem{Text = "Select", Value = "0"});
            foreach (var item in colRoleSelectList)
            {
                SelectRoleListItems.Add(new SelectListItem{Text = item.Name.ToString(), Value = item.Name.ToString()});
            }
            return SelectRoleListItems;
        }
        #endregion

        #region private ExpandedUserDTO UpdateDTOUser(ExpandedUserDTO objExpandedUserDTO)
        private ExpandedUserDTO UpdateDTOUser(ExpandedUserDTO paramExpandedUserDTO)
        {
            ApplicationUser result = UserManager.FindByName(paramExpandedUserDTO.UserName);
            // If we could not find the user, throw an exception
            if (result == null)
            {
                throw new Exception("Could not find the User");
            }
            result.Email = paramExpandedUserDTO.Email;
            // Lets check if the account needs to be unlocked
            if (UserManager.IsLockedOut(result.Id))
            {
                // Unlock user
                UserManager.ResetAccessFailedCountAsync(result.Id);
            }
            UserManager.Update(result);
            // Was a password sent across?
            if (!string.IsNullOrEmpty(paramExpandedUserDTO.Password))
            {
                // Remove current password
                var removePassword = UserManager.RemovePassword(result.Id);
                if (removePassword.Succeeded)
                {
                    // Add new password
                    var AddPassword =
                        UserManager.AddPassword(
                            result.Id,
                            paramExpandedUserDTO.Password
                            );
                    if (AddPassword.Errors.Count() > 0)
                    {
                        throw new Exception(AddPassword.Errors.FirstOrDefault());
                    }
                }
            }
            return paramExpandedUserDTO;
        }
        #endregion
        /*
        private CodeAssignTable UpdateCodeAssign(CodeAssignTable paramCodeAssign)
        {
            ApplicationUser result = UserManager.FindByName(paramCodeAssign.Email);
            CodeAssignTable res = GetCodeAssignUser(paramCodeAssign.Email);
            // If we could not find the user, throw an exception
            if (result == null)
            {
                throw new Exception("Could not find the User");
            }
            result.Email = paramCodeAssign.Email;
            res.Email = paramCodeAssign.Email;
            res.ChildCode = paramCodeAssign.ChildCode;
            
            UserManager.Update(result);
            
            
            return paramCodeAssign;
        }
        */

        #region private void DeleteUser(ExpandedUserDTO paramExpandedUserDTO)
        private void DeleteUser(ExpandedUserDTO paramExpandedUserDTO)
        {
            ApplicationUser user =
                UserManager.FindByName(paramExpandedUserDTO.UserName);
            // If we could not find the user, throw an exception
            if (user == null)
            {
                throw new Exception("Could not find the User");
            }
            UserManager.RemoveFromRoles(user.Id, UserManager.GetRoles(user.Id).ToArray());
            UserManager.Update(user);
            UserManager.Delete(user);
        }
        #endregion

        #region private ExpandedUserDTO GetUser(string paramUserName)
        private ExpandedUserDTO GetUser(string paramUserName)
        {
            ExpandedUserDTO objExpandedUserDTO = new ExpandedUserDTO();

            var result = UserManager.FindByName(paramUserName);

            // If we could not find the user, throw an exception
            if (result == null) throw new Exception("Could not find the User");

            objExpandedUserDTO.UserName = result.UserName;
            objExpandedUserDTO.Email = result.Email;
            objExpandedUserDTO.LockoutEndDateUtc = result.LockoutEndDateUtc;
            objExpandedUserDTO.AccessFailedCount = result.AccessFailedCount;
            objExpandedUserDTO.PhoneNumber = result.PhoneNumber;

            return objExpandedUserDTO;
        }
        #endregion

        #region private UserAndRolesDTO GetUserAndRoles(string UserName)
        private UserAndRolesDTO GetUserAndRoles(string UserName)
        {
            // Go get the User
            ApplicationUser user = UserManager.FindByName(UserName);
            List<UserRoleDTO> colUserRoleDTO = (from objRole in UserManager.GetRoles(user.Id)
                 select new UserRoleDTO
                 {
                     RoleName = objRole,
                     UserName = UserName
                 }).ToList();
            if (colUserRoleDTO.Count() == 0)
            {
                colUserRoleDTO.Add(new UserRoleDTO { RoleName = "No Roles Found" });
            }
            ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));
            // Create UserRolesAndPermissionsDTO
            UserAndRolesDTO objUserAndRolesDTO =
                new UserAndRolesDTO();
            objUserAndRolesDTO.UserName = UserName;
            objUserAndRolesDTO.colUserRoleDTO = colUserRoleDTO;
            return objUserAndRolesDTO;
        }
        #endregion

        #region private List<string> RolesUserIsNotIn(string UserName)
        private List<string> RolesUserIsNotIn(string UserName)
        {
            // Get roles the user is not in
            var colAllRoles = RoleManager.Roles.Select(x => x.Name).ToList();
            // Go get the roles for an individual
            ApplicationUser user = UserManager.FindByName(UserName);
            // If we could not find the user, throw an exception
            if (user == null)
            {
                throw new Exception("Could not find the User");
            }
            var colRolesForUser = UserManager.GetRoles(user.Id).ToList();
            var colRolesUserInNotIn = (from objRole in colAllRoles
                                       where !colRolesForUser.Contains(objRole)
                                       select objRole).ToList();
            if (colRolesUserInNotIn.Count() == 0)
            {
                colRolesUserInNotIn.Add("No Roles Found");
            }
            return colRolesUserInNotIn;
        }
        #endregion

        #endregion

        private CodeAssignTable GetCodeAssignUser(string email)
        {
            CodeAssignTable objCodeATable = new CodeAssignTable();

            var result = UserManager.FindByEmail(email);

            // If we could not find the user, throw an exception
            if (result == null) throw new Exception("Could not find the User");


            objCodeATable.Email = result.Email;

            return objCodeATable;
        }

        
        private void addCodeToList(CodeAssignTable code)
        {
            needsUpdate.Add(code);
        }

        private CodeAssignTable pullFromList()
        {
            if(needsUpdate != null)
            {
                CodeAssignTable ret = needsUpdate.First();
                needsUpdate.Clear();
                return ret;
            }
            else
            {
                return null;
            }
        }

        /*
        public ActionResult Generate(string email)
        {
            Random rand = new Random();
            int code = rand.Next(10000, 100000); //add if code exists condition
            int id = db.CodeTables.Count();
            string codeS = code.ToString();
            CodeAssignTable cat = GetCodeAssignUser(email);
            if (cat == null)
            {
                return HttpNotFound();
            }
            cat.ChildCode = codeS;
            CodeTable entryAdd = new CodeTable
            {
                Id = id,
                ChildCode = codeS
            };
            db.CodeTables.Add(entryAdd);
            db.Entry(cat).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        */

        public String Generate()
        {
            Random rand = new Random();
            int code = rand.Next(10000, 100000);
            return code.ToString();
        }
    }
}