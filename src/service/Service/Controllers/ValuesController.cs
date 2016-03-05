using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.Http.Cors;

namespace Service.Controllers
{
    public class ValuesController : ApiController
    {
        CardBoard cardBoard;
        List<CardBoard> li = new List<CardBoard>();
        string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IEnumerable<CardBoard> Get()
        {

            DataSet ds = new DataSet();
            SqlCommand sqlCommand = new SqlCommand();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {

                sqlCommand.Connection = new SqlConnection(connectionString);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "spGetAllLinks";
                SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                da.Fill(ds);
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                cardBoard = new CardBoard();
                cardBoard.AppID = Convert.ToInt32(dr["AppID"]);
                cardBoard.AppName = dr["AppName"].ToString();
                cardBoard.AppLinks = dr["AppLinks"].ToString();
                li.Add(cardBoard);
            }

            return li;
        }

        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Get(string name, string url)
        {
            int count = 0;
            // should probably check mail and pw for empty strings and nulls
            SqlCommand sqlCommand = new SqlCommand();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "spSavelinks";
                sqlCommand.Parameters.Add(new SqlParameter("@appname", name));
                sqlCommand.Parameters.Add(new SqlParameter("@applinks", url));
                sqlConnection.Open();
                count = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

            }

            return this.Json(count);
        }

        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public void Post([FromBody]List<string> appName)
        {
            int count = 0;
            SqlCommand sqlCommand = new SqlCommand();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {

                sqlCommand.Connection = new SqlConnection(connectionString);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "spSavelinks";
                sqlCommand.Parameters.Add(new SqlParameter("@appname", appName[0]));
                sqlCommand.Parameters.Add(new SqlParameter("@applinks", appName[1]));
                sqlConnection.Open();
                count = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

            }

        }
        #region Commented Code
        //// GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
        #endregion

    }
}
