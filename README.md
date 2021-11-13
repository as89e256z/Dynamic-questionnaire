# Dynamic-questionnaire
填問卷系統
原計畫
VS				
頁面整理		呈現內容/動作	
ConfiremationPage
使用者確認	寫完問卷要跳/否則跳回問卷內頁(答案也要回來)	
Login
登入Admin		
QuestionnaireContent
問卷內容	填寫問卷內頁	
QuestionnaireList
問卷列表	全問卷	
Statistics
統計頁	該問卷問題統計(圖表)	

AdminQuestionnaireContent
系統問卷內容	新增/修改問卷基本資料	
AdminQuestionnaireList
系統問卷列表	僅列出該系統員擁有的問卷	
AdminQuestionnaireQuestion
系統問卷問題	該問卷問題(可編輯/新增)	
AdminQuestionnaireStatistics
系統問卷問題統計	該問卷問題統計	
FillerAns	
填寫人答案	該填寫人答案	
QuestionnaireFillerList
問卷填寫人	該問卷的填寫人一覽	
FrequentlyAskedManagementPage
常用問題管理頁	僅列出該系統員擁有的問題的常用	



現在狀態

VS				
頁面整理		當前功能描述	當前問題
ConfiremationPage
使用者確認		帶入上頁填入內容、送出見可完成填表	空白也可以進，送出沒有檢查使用者可輸入值、
Login
登入Admin		檢查輸入、登入後台	
QuestionnaireContent
問卷內容		URL帶ID、以ID取生該題目	沒檢查內容、必填不必填都看不到、
QuestionnaireList
問卷列表		所有問卷一覽、搜查功能、進去問卷帶ID進氣、統計頁面帶問卷名稱進去	狀態完全沒處理
Statistics
統計頁		將資料庫表中以出現次數做計算呈百分比列出並以bootstrap列出
AdminQuestionnaireContent
系統問卷內容		修改問卷基本資料	
AdminQuestionnaireList
系統問卷列表		該管理員的問卷一覽，可搜索、ADD鍵按下可修改問卷基本內容、表上按鍵皆可帶各ID進去、刪除功能需點擊勾勾方可刪除	
AdminQuestionnaireQuestion
系統問卷問題		輸入上空格並按下加入鍵可加入問題、加入問題會直接從表下加入(無論該表原值)	常用問題無可用功能(僅可設定為常用)、加入太多SESSION、
AdminQuestionnaireStatistics
系統問卷問題統計		將資料庫表中以出現次數做計算呈百分比列出	
FillerAns
填寫人答案		看的到答案、返回鍵用得是回上一頁	
QuestionnaireFillerList
問卷填寫人		呈現該問卷填表人一覽、表上兩個按鍵相同功能可帶參數進入	
FrequentlyAskedManagementPage
常用問題管理頁		僅列出該系統員擁有的問題的常用	無法管理常用問題





