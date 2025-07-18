using Microsoft.AspNetCore.Mvc; // 使用 ASP.NET Core 的 MVC 控制器功能
using Microsoft.AspNetCore.Http; // 使用 HTTP 功能，例如 Session
using dogwebMVC.Models; // 匯入你自己的資料模型命名空間（包含 AppDbContext 與 Member）
using System; // 提供基本型別和例外處理
using System.Linq; // 提供 LINQ 查詢支援

namespace dogwebMVC.Controllers
{
    // 標記此控制器為 API 控制器，回傳 JSON 格式
    [ApiController]
    // 設定路由為 api/AuthApi（依照控制器名稱自動產生）
    [Route("api/[controller]")]
    public class AuthApiController : ControllerBase // 繼承 ControllerBase（不包含 View 功能）
    {
        private readonly AppDbContext _context; // 資料庫上下文，用來存取資料表

        // 建構子：注入資料庫上下文（透過依賴注入）
        public AuthApiController(AppDbContext context)
        {
            // 如果 context 為 null，拋出例外
            _context = context ?? throw new ArgumentNullException(nameof(context), "資料庫尚未設定");
        }

        // POST: api/AuthApi/register - 用來處理註冊
        [HttpPost("register")]
        public IActionResult Register()
        {
            try
            {
                // 從 POST 表單中取得使用者名稱與密碼欄位
                string username = Request.Form["username"].FirstOrDefault() ?? string.Empty;
                string password = Request.Form["password"].FirstOrDefault() ?? string.Empty;
                string email = Request.Form["email"].FirstOrDefault() ?? string.Empty;

                // 檢查是否為空值
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest("請輸入帳號、密碼與電子郵件");
                }

                // 檢查資料庫是否已有該帳號
                if (_context.Members.Any(m => m.Username == username))
                {
                    return BadRequest("帳號已存在");
                }

                // 建立新的會員物件
                var newMember = new Member
                {
                    Username = username,
                    Password = password,
                    Email = email
                };

                // 加入資料庫並儲存
                _context.Members.Add(newMember);
                _context.SaveChanges();

                // 回傳成功訊息與帳號
                return Ok(new { message = "註冊成功", username });
            }
            catch (Exception ex)
            {
                // 捕捉例外並回傳錯誤訊息
                return StatusCode(500, "伺服器錯誤: " + ex.Message);
            }
        }

        // POST: api/AuthApi/login - 處理登入
        [HttpPost("login")]
        public IActionResult Login()
        {
            try
            {
                // 取得登入表單的帳號與密碼
                string username = Request.Form["username"].FirstOrDefault() ?? string.Empty;
                string password = Request.Form["password"].FirstOrDefault() ?? string.Empty;

                // 驗證是否為空
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return BadRequest("請輸入帳號與密碼");
                }

                // 從資料庫查詢該帳號
                var member = _context.Members.FirstOrDefault(m => m.Username == username);
                if (member == null)
                {
                    return BadRequest("無此帳號");
                }

                // 比對密碼是否正確
                if (member.Password != password)
                {
                    return Unauthorized("密碼錯誤");
                }

                // 寫入 Session，紀錄使用者登入狀態
                HttpContext.Session.SetString("user", username);


                // 回傳成功訊息
                return Ok(new { message = "登入成功", username });
            }
            catch (Exception ex)
            {
                // 捕捉例外錯誤
                return StatusCode(500, "伺服器錯誤: " + ex.Message);
            }

        }

        // GET: api/AuthApi/check - 檢查登入狀態
        [HttpGet("check")]
        public IActionResult CheckLogin()
        {
 var username = HttpContext.Session.GetString("user");
    if (string.IsNullOrEmpty(username))
    {
        return Ok("未登入");
    }

    var isAdmin = IsAdmin(username);
    return Ok(new
    {
        loggedIn = true,
        isAdmin = isAdmin,
        username = username,
        message = "歡迎"
    });
        }

        // GET: api/AuthApi/getUser - 取得使用者資訊
        [HttpGet("getUser")]
        public IActionResult GetUser()
        {
            var username = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(username))
            {
                return Ok(new { message = "未登入" });
            }

            var member = _context.Members.FirstOrDefault(m => m.Username == username);
            if (member == null)
            {
                return NotFound("使用者不存在");
            }

            return Ok(new
            {
                member.Id,
                member.Username,
                member.Email,
                isAdmin = IsAdmin(username)
    });
        }

        // POST: api/AuthApi/logout - 處理登出
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // 移除 Session 中的使用者資料
            HttpContext.Session.Remove("user");

            // 回傳成功訊息
            return Ok("已登出");
        }
private bool IsAdmin(string username)
{
    var admins = new List<string> { "yenting", "user", "root" };
    return admins.Any(a => a.Equals(username, StringComparison.OrdinalIgnoreCase));
}


    }
    
}
