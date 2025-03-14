

using BusinessLayer.Services;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace APP_PROJECT.Controllers
{
    public class UserController : Controller
    {
        private IUserService userService;

        // 의존성 주입 (program.cs 참조)
        public UserController(IUserService service)
        {
            userService = service;
        }

        public IActionResult Index()
        {
            return View("User/UserList");
        }

        public async Task<IActionResult> UserList()
        {
            var users = await userService.GetAllUsers();

            // 뷰로 데이터 전달
            return View(users);
        }

        public async Task<IActionResult> UserDetail(int id)
        {
            try
            {
                var user = await userService.GetUserById(id);
                if (user == null)
                    return NotFound("User not found");

                return View(user);
            }
            catch (Exception ex)
            {
                // 로그 기록 후 에러 페이지 반환
                return StatusCode(500, "Internal Server Error");
            }
        }

        public async Task<IActionResult> NewUser(int id)
        {
            if (id == 0) // 등록(Create)
            {
                ViewData["title2"] = "신청자 등록";  // 등록 페이지로 제목 설정
                return View(new USER()); // 빈 모델 전달
            }

            // 수정(Edit): id에 해당하는 유저 데이터 로드
            var user = await userService.GetUserById(id);
            if (user == null)
            {
                return NotFound(); // id에 해당하는 데이터가 없으면 404
            }
            ViewData["title2"] = "신청자 수정";  // 수정 페이지로 제목 설정
            return View(user); // 모델 데이터를 뷰로 전달
        }

        [HttpDelete]
        public async Task<IActionResult> CreateUser(UserDTO createUserDTO)
        {
            if (createUserDTO.Id == 0) // 등록
            {
                await userService.CreateUser(createUserDTO);
            }
            else // 수정
            {
                await userService.UpdateUser(createUserDTO);
            }
            return Redirect("/user/userlist");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await userService.DeleteUser(id);
            return Redirect("/user/userlist");
        }
    }
}
