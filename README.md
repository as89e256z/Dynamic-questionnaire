# Dynamic-questionnaire
<h1>填問卷系統</h1>


<h3>原計畫</h3>

頁面整理    

頁面名稱<br/>呈現內容/動作	

ConfiremationPage<br/>
使用者確認	寫完問卷要跳/否則跳回問卷內頁(答案也要回來)	

Login<br/>
登入Admin		

QuestionnaireContent<br/>
問卷內容	填寫問卷內頁	

QuestionnaireList<br/>
問卷列表	全問卷	

Statistics<br/>
統計頁	該問卷問題統計(圖表)	

AdminQuestionnaireContent<br/>
系統問卷內容	新增/修改問卷基本資料	

AdminQuestionnaireList<br/>
系統問卷列表	僅列出該系統員擁有的問卷	

AdminQuestionnaireQuestion<br/>
系統問卷問題	該問卷問題(可編輯/新增)	

AdminQuestionnaireStatistics<br/>
系統問卷問題統計	該問卷問題統計	

FillerAns	<br/>
填寫人答案	該填寫人答案	

QuestionnaireFillerList<br/>
問卷填寫人	該問卷的填寫人一覽	

FrequentlyAskedManagementPage<br/>
常用問題管理頁	僅列出該系統員擁有的問題的常用	




<h3>現在狀態</h3>


頁面整理		

當前功能描述<br/>
當前問題

ConfiremationPage<br/>
使用者確認		帶入上頁填入內容、送出見可完成填表	空白也可以進，送出沒有檢查使用者可輸入值

Login<br/>
登入Admin		檢查輸入、登入後台	

QuestionnaireContent<br/>
問卷內容		URL帶ID、以ID取生該題目	沒檢查內容、必填不必填都看不到、

QuestionnaireList<br/>
問卷列表		所有問卷一覽、搜查功能、進去問卷帶ID進氣、統計頁面帶問卷名稱進去	狀態完全沒處理

Statistics<br/>
統計頁		將資料庫表中以出現次數做計算呈百分比列出並以bootstrap列出

AdminQuestionnaireContent<br/>
系統問卷內容		修改問卷基本資料	

AdminQuestionnaireList<br/>
系統問卷列表		該管理員的問卷一覽，可搜索、ADD鍵按下可修改問卷基本內容、表上按鍵皆可帶各ID進去、刪除功能需點擊勾勾方可刪除	

AdminQuestionnaireQuestion<br/>
系統問卷問題		輸入上空格並按下加入鍵可加入問題、加入問題會直接從表下加入(無論該表原值)	常用問題無可用功能(僅可設定為常用)、加入太多SESSION

AdminQuestionnaireStatistics<br/>
系統問卷問題統計		將資料庫表中以出現次數做計算呈百分比列出	

FillerAns<br/>
填寫人答案		看的到答案、返回鍵用得是回上一頁	

QuestionnaireFillerList<br/>
問卷填寫人		呈現該問卷填表人一覽、表上兩個按鍵相同功能可帶參數進入	

FrequentlyAskedManagementPage<br/>
常用問題管理頁		僅列出該系統員擁有的問題的常用	無法管理常用問題






