using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dynamic_questionnaire.DBModels;

namespace Dynamic_questionnaire.DB
{
    public class DBHelper
    {
        public static string GetConnectionString()
        {
            string val = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            return val;
        }

        /// <summary>
        /// 查Admin帳戶
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static DataRow GetUserInfoByAccount(string account)
        {
            string connectionString = GetConnectionString();
            string dbCommandString =
                @"SELECT [UserID] ,[Account] ,[PassWord]
                    FROM Admin
                    WHERE [Account] = @account";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(dbCommandString, conn))
                {
                    comm.Parameters.AddWithValue("@account", account);
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = comm.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        reader.Close();

                        if (dt.Rows.Count == 0)
                            return null;

                        DataRow dr = dt.Rows[0];
                        return dr;
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                        return null;
                    }
                }
            }

        }


        public static DataTable GetDataTable(string dbCommand, List<SqlParameter> parameters)
        {
            string connectionString = GetConnectionString();


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(dbCommand, conn))
                {
                    comm.Parameters.AddRange(parameters.ToArray());

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = comm.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        reader.Close();

                        return dt;
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                        return null;
                    }
                }
            }

        }

        public static List<DBModels.Questionnaire> GetAccountQuestionName(string account)
        {
            string connectionString = GetConnectionString();
            string dbCommandString =
                @"SELECT  [QuestionnaireName]
                    FROM [Dynamic questionnaire].[dbo].[Questionnaire]
                    WHERE [CreateAccount] = @account";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(dbCommandString, conn))
                {
                    comm.Parameters.AddWithValue("@account", account);
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = comm.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        List<DBModels.Questionnaire> list = new List<DBModels.Questionnaire>();

                        foreach (DataRow dr in dt.Rows)
                        {
                            DBModels.Questionnaire model = new DBModels.Questionnaire();
                            model.QuestionnaireName = (string)dr["QuestionnaireName"];

                            list.Add(model);
                        }
                        return list;
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                        return null;
                    }
                }
            }
        }



        ///
        public static DataTable GetQuestionnaireTable(string qustnirename,
            string _strarttime, string _endtime)
        {
            string connectionString = GetConnectionString();
            string dbCommandString =
                @"SELECT [QuestionnaireNumber]
                          ,[QuestionnaireName]
                          ,[State]
                          ,[StartTime]
                          ,[EndTime]
                    FROM Questionnaire";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(dbCommandString, conn))
                {
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = comm.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        return dt;
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                        return null;
                    }
                }
            }

        }

        /// <summary>
        /// 查問卷清單
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static List<DBModels.Questionnaire> GetQuestionnaireList(string account, string qustnirename,
            string _strarttime, string _endtime)
        {
            try
            {
                //----- Process filter conditions -----
                List<string> conditions = new List<string>();
                DateTime starttime = new DateTime();
                DateTime endtime = new DateTime();
                if (!string.IsNullOrEmpty(account))
                    conditions.Add(" [CreateAccount] = @account ");
                if (!string.IsNullOrEmpty(qustnirename))
                    conditions.Add(" [QuestionnaireName] LIKE '%' + @qustnirename + '%'");
                //ToString("yyyy-MM-dd" + "T" + "hh:mm")               

                if (!string.IsNullOrEmpty(_strarttime) && DateTime.TryParseExact
                    (_strarttime, "yyyy-MM-dd" + "T" + "hh:mm", null, System.Globalization.DateTimeStyles.None, out starttime))
                {
                    conditions.Add(" [StartTime] > @starttime");
                    conditions.Add(" [EndTime] > @starttime1");
                }              
                if (!string.IsNullOrEmpty(_endtime) && DateTime.TryParseExact(_endtime, "yyyy-MM-dd" + "T" + "hh:mm", null, System.Globalization.DateTimeStyles.None, out endtime))
                {
                    conditions.Add(" [EndTime] < @endtime");
                    conditions.Add(" [StartTime] < @endtime1");
                }

                string filterConditions =
                    (conditions.Count > 0)
                        ? ("WHERE" + string.Join(" AND ", conditions))
                        : string.Empty;
                //----- Process filter conditions -----

                string query =
                    $@" SELECT [QuestionnaireNumber]
                          ,[QuestionnaireName]
                          ,[QuestionnaireDescribe]
                          ,[CreateAccount]
                          ,[State]
                          ,[StartTime]
                          ,[EndTime]
                          ,[CreateTime]
                        FROM Questionnaire
                        {filterConditions}
                    ORDER BY [QuestionnaireNumber] DESC
                    ";

                List<SqlParameter> dbParameters = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(account))
                    dbParameters.Add(new SqlParameter("@account", account));
                if (!string.IsNullOrEmpty(qustnirename))
                    dbParameters.Add(new SqlParameter("@qustnirename", qustnirename));
                if (DateTime.TryParse(_strarttime, out starttime))
                {
                    dbParameters.Add(new SqlParameter("@starttime", starttime));
                    dbParameters.Add(new SqlParameter("@starttime1", starttime));
                }
                if (DateTime.TryParse(_endtime, out endtime))
                {
                    dbParameters.Add(new SqlParameter("@endtime", endtime));
                    dbParameters.Add(new SqlParameter("@endtime1", endtime));
                }


                DataTable dt = DB.DBHelper.GetDataTable(query, dbParameters);

                List<DBModels.Questionnaire> list = new List<DBModels.Questionnaire>();


                foreach (DataRow dr in dt.Rows)
                {
                    DBModels.Questionnaire model = new DBModels.Questionnaire();
                    model.QuestionnaireNumber = (int)dr["QuestionnaireNumber"];
                    model.QuestionnaireName = (string)dr["QuestionnaireName"];
                    model.QuestionnaireDescribe = (string)dr["QuestionnaireName"];
                    model.CreateAccount = (string)dr["CreateAccount"];
                    model.State = (int)dr["State"];
                    model.StartTime = (DateTime)dr["StartTime"];
                    model.EndTime = (DateTime)dr["EndTime"];
                    model.CreateTime = (DateTime)dr["CreateTime"];

                    list.Add(model);
                }
                return list;

                //using (DBModels.ContextModel context = new DBModels.ContextModel())
                //{
                //    var query =
                //        (from item in context.Questionnaires
                //         where item.CreateAccount == account && item.QuestionnaireName == qustnirename
                //         && item.StartTime == starttime && item.EndTime == endtime
                //         select item);

                //    var list = query.ToList();
                //    return list;
                //}
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Filler
        public static DataTable GetFillerTable(string name, string questionnairetitle)
        {
            string connectionString = GetConnectionString();
            string dbCommandString =
                @"SELECT [Name]
                          ,[Phone]
                          ,[Email]
                          ,[Ages]
                          ,[QuestionnaireTitle]
                          ,[ProblemTitle]
                          ,[CreateTime]
                          ,[Ans1]
                          ,[Ans2]
                          ,[Ans3]
                          ,[Ans4]
                          ,[Ans5]
                          ,[Ans6]
                          ,[Ans7]
                          ,[Ans8]
                          ,[Ans9]
                      FROM [Dynamic questionnaire].[dbo].[Filler]
                     WHERE [Name] = @name AND [QuestionnaireTitle]  = @questionnairetitle";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(dbCommandString, conn))
                {
                    comm.Parameters.AddWithValue("@name", name);
                    comm.Parameters.AddWithValue("@questionnairetitle", questionnairetitle);
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = comm.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        reader.Close();

                        return dt;

                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                        return null;
                    }
                }
            }


        }

        public static DataTable GetFillersAnsTable(string questionnairetitle)
        {
            string connectionString = GetConnectionString();
            string dbCommandString =
                @"SELECT [Name]
                          ,[Phone]
                          ,[Email]
                          ,[Ages]
                          ,[QuestionnaireTitle]
                          ,[ProblemTitle]
                          ,[CreateTime]
                          ,[Ans1]
                          ,[Ans2]
                          ,[Ans3]
                          ,[Ans4]
                          ,[Ans5]
                          ,[Ans6]
                          ,[Ans7]
                          ,[Ans8]
                          ,[Ans9]
                      FROM [Dynamic questionnaire].[dbo].[Filler]
                     WHERE [QuestionnaireTitle]  = @questionnairetitle";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(dbCommandString, conn))
                {
                    comm.Parameters.AddWithValue("@questionnairetitle", questionnairetitle);
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = comm.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        reader.Close();

                        return dt;

                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                        return null;
                    }
                }
            }


        }



        public static DataTable GetFillerNameList(string questionnairetitle)
        {
            string connectionString = GetConnectionString();
            string dbCommandString =
                @"SELECT DISTINCT  
		                [Name]
                        ,[CreateTime]
                    FROM [Dynamic questionnaire].[dbo].[Filler]
                    WHERE [QuestionnaireTitle] = @questionnairetitle";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(dbCommandString, conn))
                {
                    comm.Parameters.AddWithValue("@questionnairetitle", questionnairetitle);
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = comm.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        reader.Close();

                        return dt;
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                        return null;
                    }
                }
            }
        }

        public static List<DBModels.Filler> GetFillerList(string questionnairetitle)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.Fillers
                         where item.QuestionnaireTitle == questionnairetitle
                         select item);

                    var list = query.ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }

        }

        #endregion

        public static DataRow GetQuestionnaire(string questionnairenumber)
        {
            string connectionString = GetConnectionString();
            string dbCommandString =
                @"SELECT [QuestionnaireNumber]
                          ,[QuestionnaireName]
                          ,[QuestionnaireDescribe]
                          ,[CreateAccount]
                          ,[State]
                          ,[StartTime]
                          ,[EndTime]
                          ,[CreateTime]
                    FROM Questionnaire
                    WHERE [QuestionnaireNumber] = @questionnairenumber";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(dbCommandString, conn))
                {
                    comm.Parameters.AddWithValue("@questionnairenumber", questionnairenumber);
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = comm.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        reader.Close();

                        if (dt.Rows.Count == 0)
                            return null;

                        DataRow dr = dt.Rows[0];
                        return dr;
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                        return null;
                    }
                }
            }

        }

        public static bool UpdateQuestionnaire(int questionnairenumber, string questionnairename, string questionnairedescribe, int state, DateTime starttime, DateTime endtime)
        {
            string connectionString = GetConnectionString();
            string dbCommandString =
                @"UPDATE [dbo].[Questionnaire]
                       SET [QuestionnaireName] = @questionnairename
                          ,[QuestionnaireDescribe] = @questionnairedescribe
                          ,[State] = @state
                          ,[StartTime] = @starttime
                          ,[EndTime] = @endtime
                     WHERE QuestionnaireNumber = @questionnairenumber";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(dbCommandString, conn))
                {
                    comm.Parameters.AddWithValue("@questionnairename", questionnairename);
                    comm.Parameters.AddWithValue("@questionnairedescribe", questionnairedescribe);
                    comm.Parameters.AddWithValue("@state", state);
                    comm.Parameters.AddWithValue("@starttime", starttime);
                    comm.Parameters.AddWithValue("@endtime", endtime);
                    comm.Parameters.AddWithValue("@questionnairenumber", questionnairenumber);
                    try
                    {
                        conn.Open();
                        int isTrue = comm.ExecuteNonQuery();
                        if (isTrue == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                        return false;
                    }
                }
            }

        }
        public static void CreateQuestionnaireContent(string questionnairename, string questionnairedescribe, string createaccount, int state, DateTime starttime, DateTime endtime, DateTime createtime)
        {
            string connectionString = GetConnectionString();
            string dbCommandString =
                @"INSERT INTO [dbo].[Questionnaire]
                        (                            
                              [QuestionnaireName]
                              ,[QuestionnaireDescribe]
                              ,[CreateAccount]
                              ,[State]
                              ,[StartTime]
                              ,[EndTime]
                              ,[CreateTime]
                        )
                        VALUES
                        (
                            @questionnairename
                            ,@questionnairedescribe
                            ,@createaccount
                            ,@state                            
                            ,@starttime
                            ,@endtime
                            ,@createtime
                        )";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(dbCommandString, conn))
                {
                    comm.Parameters.AddWithValue("@questionnairename", questionnairename);
                    comm.Parameters.AddWithValue("@questionnairedescribe", questionnairedescribe);
                    comm.Parameters.AddWithValue("@createaccount", createaccount);
                    comm.Parameters.AddWithValue("@state", state);
                    comm.Parameters.AddWithValue("@starttime", starttime);
                    comm.Parameters.AddWithValue("@endtime", endtime);
                    comm.Parameters.AddWithValue("@createtime", createtime);
                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                    }
                }
            }

        }



        public static void DeleteQuestionnaire(int QuestionnaireNumber)
        {
            string connectionString = GetConnectionString();
            string dbCommandString =
                @"DELETE FROM [dbo].[Questionnaire]
                  WHERE [QuestionnaireNumber] = @questionnairenumber
                        ";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(dbCommandString, conn))
                {
                    comm.Parameters.AddWithValue("@questionnairenumber", QuestionnaireNumber);//
                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                    }
                }
            }

        }

        public static void DeleteQuestionnaireArray(string[] QuestionnaireNumber)
        {
            string connectionString = GetConnectionString();
            var _strnbr = "";
            int count = 0;
            for (int i = 0; i < QuestionnaireNumber.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(QuestionnaireNumber[i]))
                {
                    if (count > 0)
                    {
                        _strnbr += $",@questionnairenumber{i}";
                    }
                    else
                    {
                        count++;
                        _strnbr += $"@questionnairenumber{i}";
                    }

                }
            }
            string dbCommandString =
            $@"DELETE FROM [dbo].[Questionnaire]
                  WHERE [QuestionnaireNumber] IN ( {_strnbr} )
                        ";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(dbCommandString, conn))
                {
                    for (int i = 0; i < QuestionnaireNumber.Length; i++)
                    {
                        comm.Parameters.AddWithValue($"@questionnairenumber{i}", int.Parse(QuestionnaireNumber[i]));//
                    }
                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                    }
                }
            }

        }


        public static DataTable GetQuestionnaireQuestionList(string questionnairename)
        {
            string connectionString = GetConnectionString();
            string dbCommandString =
                @"SELECT [QuestionID]
                          ,[QuestionnaireTitle]
                          ,[ProblemID]
                          ,[ProblemTitle]
                          ,[TypeOfProblem]
                          ,[Required]
                          ,[FrequentlyAsked]
                          ,[Ans1]
                          ,[Ans2]
                          ,[Ans3]
                          ,[Ans4]
                          ,[Ans5]
                          ,[Ans6]
                          ,[Ans7]
                          ,[Ans8]
                          ,[Ans9]
                    FROM [Question]
                    WHERE [QuestionnaireTitle] = @questionnairename";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(dbCommandString, conn))
                {
                    comm.Parameters.AddWithValue("@questionnairename", questionnairename);
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = comm.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        return dt;
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                        return null;
                    }
                }
            }

        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="questionnairename"></param>
        /// <returns></returns>
        public static List<DBModels.Question> GetQuestionList(List<DBModels.Questionnaire> questionnairename)
        {
            try
            {
                string connectionString = GetConnectionString();

                List<string> conditions = new List<string>();
                List<SqlParameter> dbParameters = new List<SqlParameter>();
                int frequency = 0;



                foreach (var item in questionnairename)
                {
                    conditions.Add($"[QuestionnaireTitle] = @questionnairename{frequency} ");
                    dbParameters.Add(new SqlParameter($"@questionnairename{frequency}", item.QuestionnaireName));
                    frequency++;
                }
                string filterConditions =
                    (conditions.Count > 0)
                        ? ("WHERE" + string.Join(" OR ", conditions))
                        : string.Empty;

                string query =
                    $@" SELECT [QuestionID]
                          ,[QuestionnaireTitle]
                          ,[ProblemID]
                          ,[ProblemTitle]
                          ,[TypeOfProblem]
                          ,[Required]
                          ,[FrequentlyAsked]
                          ,[Ans1]
                          ,[Ans2]
                          ,[Ans3]
                          ,[Ans4]
                          ,[Ans5]
                          ,[Ans6]
                          ,[Ans7]
                          ,[Ans8]
                          ,[Ans9]
                    FROM [Dynamic questionnaire].[dbo].[Question]
                        {filterConditions}
                    ";


                DataTable dt = DB.DBHelper.GetDataTable(query, dbParameters);


                List<DBModels.Question> list = new List<DBModels.Question>();
                foreach (DataRow dr in dt.Rows)
                {
                    DBModels.Question model = new DBModels.Question();
                    model.QuestionID = (string)dr["QuestionID"];
                    model.QuestionnaireTitle = (string)dr["QuestionnaireTitle"];
                    model.ProblemID = (int)dr["ProblemID"];
                    model.ProblemTitle = (string)dr["ProblemTitle"];
                    model.TypeOfProblem = (int)dr["TypeOfProblem"];
                    model.Required = (bool)dr["Required"];
                    model.FrequentlyAsked = (bool)dr["FrequentlyAsked"];
                    model.Ans1 = (string)dr["Ans1"];
                    model.Ans2 = (string)dr["Ans2"];
                    model.Ans3 = (string)dr["Ans3"];
                    model.Ans4 = (string)dr["Ans4"];
                    model.Ans5 = (string)dr["Ans5"];
                    model.Ans6 = (string)dr["Ans6"];
                    model.Ans7 = (string)dr["Ans7"];
                    model.Ans8 = (string)dr["Ans8"];
                    model.Ans9 = (string)dr["Ans9"];
                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }


        public static DataRow GetQuestionnaireQuestionChoice(string questionnairename, int problemid)
        {
            string connectionString = GetConnectionString();
            string dbCommandString =
                @"SELECT [QuestionID]
                          ,[QuestionnaireTitle]
                          ,[ProblemID]
                          ,[ProblemTitle]
                          ,[TypeOfProblem]
                          ,[Required]
                          ,[FrequentlyAsked]
                          ,[Ans1]
                          ,[Ans2]
                          ,[Ans3]
                          ,[Ans4]
                          ,[Ans5]
                          ,[Ans6]
                          ,[Ans7]
                          ,[Ans8]
                          ,[Ans9]
                    FROM [Question]
                    WHERE [ProblemID] = @problemid AND [QuestionnaireTitle]=@questionnairetitle";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(dbCommandString, conn))
                {
                    comm.Parameters.AddWithValue("@questionnairetitle", questionnairename);
                    comm.Parameters.AddWithValue("@problemid", problemid);
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = comm.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        reader.Close();

                        if (dt.Rows.Count == 0)
                            return null;

                        DataRow dr = dt.Rows[0];
                        return dr;
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                        return null;
                    }
                }
            }

        }

        public static int GetCountQuestion(string questionnairename)
        {
            string connectionString = GetConnectionString();
            string dbCommandString =
                @"SELECT Count(*) as [ProblemCount]
                  FROM [Question]
                  WHERE [QuestionnaireTitle] = @questionnairetitle";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(dbCommandString, conn))
                {
                    comm.Parameters.AddWithValue("@questionnairetitle", questionnairename);
                    try
                    {
                        conn.Open();
                        int intCount = Convert.ToInt32(comm.ExecuteScalar());

                        return intCount;
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                        return -1;
                    }
                }
            }

        }

        public static string GetLastQuestionID()
        {
            string connectionString = GetConnectionString();
            string dbCommandString =
                @"SELECT MAX([QuestionID]) 
                    FROM [dbo].[Question]";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(dbCommandString, conn))
                {
                    try
                    {
                        conn.Open();
                        string  _MaxQuestionID = Convert.ToString(comm.ExecuteScalar());

                        return _MaxQuestionID;
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                        return null;
                    }
                }
            }

        }


        public static List<DBModels.Question> GetQuestionChoice(string problemID)
        {
            try
            {
                using (DBModels.ContextModel context = new DBModels.ContextModel())
                {
                    var query =
                        (from item in context.Questions
                         where item.QuestionID == problemID
                         select item);

                    var list = query.ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                throw;
            }
        }

        public static bool UpdataQuestionChoice
            (string questionid, string questionnairetitle, int problemid, string problemtitle, int typeofproblem,
            bool required, bool frequentlyasked, string ans1, string ans2, string ans3, string ans4, string ans5, string ans6,
            string ans7, string ans8, string ans9)
        {
            string connectionString = GetConnectionString();
            string dbCommandString =
                @"UPDATE [dbo].[Question]
                       SET  [QuestionID] = @questionid
                         ,[QuestionnaireTitle] = @questionnairetitle
                         ,[ProblemID] = @problemid
                         ,[ProblemTitle] = @problemtitle
                         ,[TypeOfProblem] = @typeofproblem
                         ,[Required] = @required
                         ,[FrequentlyAsked] = @frequentlyasked
                         ,[Ans1] = @ans1
                         ,[Ans2] = @ans2
                         ,[Ans3] = @ans3
                         ,[Ans4] = @ans4
                         ,[Ans5] = @ans5
                         ,[Ans6] = @ans6
                         ,[Ans7] = @ans7
                         ,[Ans8] = @ans8
                         ,[Ans9] = @ans9
                    WHERE [QuestionID] = @questionid AND [QuestionnaireTitle]　= @questionnairetitle";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(dbCommandString, conn))
                {
                    comm.Parameters.AddWithValue("@questionid", questionid);
                    comm.Parameters.AddWithValue("@questionnairetitle", questionnairetitle);
                    comm.Parameters.AddWithValue("@problemid", problemid);
                    comm.Parameters.AddWithValue("@problemtitle", problemtitle);
                    comm.Parameters.AddWithValue("@typeofproblem", typeofproblem);
                    comm.Parameters.AddWithValue("@required", required);
                    comm.Parameters.AddWithValue("@frequentlyasked", frequentlyasked);
                    comm.Parameters.AddWithValue("@ans1", ans1);
                    comm.Parameters.AddWithValue("@ans2", ans2);
                    comm.Parameters.AddWithValue("@ans3", ans3);
                    comm.Parameters.AddWithValue("@ans4", ans4);
                    comm.Parameters.AddWithValue("@ans5", ans5);
                    comm.Parameters.AddWithValue("@ans6", ans6);
                    comm.Parameters.AddWithValue("@ans7", ans7);
                    comm.Parameters.AddWithValue("@ans8", ans8);
                    comm.Parameters.AddWithValue("@ans9", ans9);
                    try
                    {
                        conn.Open();
                        int isTrue = comm.ExecuteNonQuery();
                        if (isTrue == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                        return false;
                    }
                }
            }

        }

        public static bool UpdateQuestionChoice(DBModels.Question list)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var dbObject =
                        context.Questions.Where(obj => obj.QuestionID == list.QuestionID).FirstOrDefault();

                    if (dbObject != null)
                    {
                        dbObject.QuestionnaireTitle = list.QuestionnaireTitle;
                        dbObject.ProblemID = list.ProblemID;
                        dbObject.ProblemTitle = list.ProblemTitle;
                        dbObject.TypeOfProblem = list.TypeOfProblem;
                        dbObject.Required = list.Required;
                        dbObject.FrequentlyAsked = list.FrequentlyAsked;
                        dbObject.Ans1 = list.Ans1;
                        dbObject.Ans2 = list.Ans2;
                        dbObject.Ans3 = list.Ans3;
                        dbObject.Ans4 = list.Ans4;
                        dbObject.Ans5 = list.Ans5;
                        dbObject.Ans6 = list.Ans6;
                        dbObject.Ans7 = list.Ans7;
                        dbObject.Ans8 = list.Ans8;
                        dbObject.Ans9 = list.Ans9;

                        context.SaveChanges();

                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return false;
            }
        }

        public static void CreateQuestionChoice(DBModels.Question list)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    context.Questions.Add(list);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }




        public static bool UpdateFillerChoice
(string name, string phone, string email, int ages,
string questionnairetitle, string problemtitle, DateTime createtime, string[] ans)
        {
            string connectionString = GetConnectionString();
            string dbCommandString =
                @"UPDATE [dbo].[Filler]
                   SET [Name]               = @name
                      ,[Phone]              = @phone
                      ,[Email]              = @email
                      ,[Ages]               = @ages              
                      ,[QuestionnaireTitle] = @questionnairetitle
                      ,[ProblemTitle]       = @problemtitle
                      ,[CreateTime]         = @createtime
                      ,[Ans1]               = @ans1
                      ,[Ans2]               = @ans2
                      ,[Ans3]               = @ans3
                      ,[Ans4]               = @ans4
                      ,[Ans5]               = @ans5
                      ,[Ans6]               = @ans6
                      ,[Ans7]               = @ans7
                      ,[Ans8]               = @ans8
                      ,[Ans9]               = @ans9
                    WHERE [Name]  = @name AND [QuestionnaireTitle] = @questionnairetitle AND [ProblemTitle] = @problemtitle
                 ";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(dbCommandString, conn))
                {
                    comm.Parameters.AddWithValue("@name", name);
                    comm.Parameters.AddWithValue("@phone", phone);
                    comm.Parameters.AddWithValue("@email", email);
                    comm.Parameters.AddWithValue("@ages", ages);
                    comm.Parameters.AddWithValue("@questionnairetitle", questionnairetitle);
                    comm.Parameters.AddWithValue("@problemtitle", problemtitle);
                    comm.Parameters.AddWithValue("@createtime", createtime);

                    for (int i = 0; i < ans.Length; i++)
                    {
                        comm.Parameters.AddWithValue($"@ans{i + 1}", ans[i]);
                    }

                    try
                    {
                        conn.Open();
                        int isTrue = comm.ExecuteNonQuery();
                        if (isTrue == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                        return false;
                    }
                }
            }

        }

        public static void InsertFillerChoice
        (string name, string phone, string email, int ages,
        string questionnairetitle, string problemtitle, DateTime createtime, string[] ans)
        {
            string connectionString = GetConnectionString();
            string dbCommandString =
                @"INSERT INTO [dbo].[Filler]
                        (                            
                               [Name]
                               ,[Phone]
                               ,[Email]
                               ,[Ages]
                               ,[QuestionnaireTitle]
                               ,[ProblemTitle]
                               ,[CreateTime]
                               ,[Ans1]
                               ,[Ans2]
                               ,[Ans3]
                               ,[Ans4]
                               ,[Ans5]
                               ,[Ans6]
                               ,[Ans7]
                               ,[Ans8]
                               ,[Ans9]
                        )
                        VALUES
                        (
                                @name
                                ,@phone
                                ,@email
                                ,@ages                            
                                ,@questionnairetitle
                                ,@problemtitle
                                ,@createtime
                                ,@ans1
                                ,@ans2
                                ,@ans3
                                ,@ans4
                                ,@ans5
                                ,@ans6
                                ,@ans7
                                ,@ans8
                                ,@ans9
                        )";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(dbCommandString, conn))
                {
                    comm.Parameters.AddWithValue("@name", name);
                    comm.Parameters.AddWithValue("@phone", phone);
                    comm.Parameters.AddWithValue("@email", email);
                    comm.Parameters.AddWithValue("@ages", ages);
                    comm.Parameters.AddWithValue("@questionnairetitle", questionnairetitle);
                    comm.Parameters.AddWithValue("@problemtitle", problemtitle);
                    comm.Parameters.AddWithValue("@createtime", createtime);

                    for (int i = 0; i < ans.Length; i++)
                    {
                        comm.Parameters.AddWithValue($"@ans{i + 1}", ans[i]);
                    }

                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                    }
                }
            }

        }



    }
}