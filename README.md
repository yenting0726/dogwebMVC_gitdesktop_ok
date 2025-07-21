# 狗狗網站（dogwebMVC）

## 專案簡介
本專案是一個以 ASP.NET Core MVC 與 Entity Framework Core 開發的寵物電商網站，提供狗狗相關產品展示、會員註冊登入、購物車、地圖查詢等功能，適合用於學習或作為寵物電商平台的起始範例。

## 主要功能
- 產品展示：瀏覽多種狗狗用品，支援圖片上傳與管理。
- 會員系統：註冊、登入、登出、會員資料查詢。
- 購物車：商品加入購物車、數量調整、清空、結帳。
- 公園地圖：查詢各地區寵物友善公園地圖。
- 管理員帳號支援（預設帳號 yenting、user、root 具管理權限）。

## 安裝與執行方式
1. **環境需求**：
   - .NET 8.0 SDK
   - SQL Server（或修改連線字串為其他資料庫）
2. **安裝步驟**：
   - 下載或 clone 此專案
   - 還原 NuGet 套件：`dotnet restore`
   - 執行資料庫遷移：`dotnet ef database update`
   - 啟動專案：`dotnet run`
   - 預設網址：http://localhost:5119

## 專案結構
- `Controllers/`：MVC 控制器（產品、會員、首頁等）
- `Models/`：資料模型（產品、會員、資料庫上下文）
- `Views/`：Razor 頁面（首頁、產品、會員、購物車、地圖等）
- `wwwroot/`：靜態資源（CSS、JS、圖片）
- `Migrations/`：Entity Framework Core 資料庫遷移檔
- `appsettings.json`：設定檔（含資料庫連線字串）

## 技術說明
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Razor View Engine
- jQuery、Vue.js（前端互動）
- Bootstrap 5（UI 樣式） 