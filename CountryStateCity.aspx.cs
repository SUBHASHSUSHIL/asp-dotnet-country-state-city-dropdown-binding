using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Country_State_City
{
    public partial class CountryStateCity : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbms"].ConnectionString.ToString());
        public string Query { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                Cmontry();
            }
        }

        private void Cmontry()
        {
            Query = "select * from Country";
            using(SqlCommand cmd = new SqlCommand(Query, con))
            {
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                ddCountry.DataSource = ds;
                ddCountry.DataTextField = "Name";
                ddCountry.DataValueField = "CId";
                ddCountry.DataBind();
                ddCountry.Items.Insert(0, new ListItem("Select Country", "0"));
            }
        }

        protected void ddCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            Query = "select * from State where CountryId = @CountryId";
            using(SqlCommand cmd = new SqlCommand(Query,con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@CountryId", ddCountry.SelectedValue.ToString());
                SqlDataAdapter adp = new SqlDataAdapter( cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                ddState.DataSource = ds;
                ddState.DataTextField = "Name";
                ddState.DataValueField = "SId";
                ddState.DataBind();
                ddState.Items.Insert(0, new ListItem("Select State", "0"));
            }
        }

        protected void ddState_SelectedIndexChanged(object sender, EventArgs e)
        {
            Query = "select * from City where StateId = @StateId";
            using(SqlCommand cmd = new SqlCommand(Query,con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@StateId", ddState.SelectedValue.ToString());
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                ddCity.DataSource = ds;
                ddCity.DataTextField = "Name";
                ddCity.DataValueField = "Id";
                ddCity.DataBind();
                ddCity.Items.Insert(0, new ListItem("Select City", "0"));
            }
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            Query = "insert into Csc(CountryN,StateN,CityN,CscName) values (@CountryN,@StateN,@CityN,@CscName)";
            using( SqlCommand cmd = new SqlCommand(Query, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@CountryN",ddCountry.SelectedValue);
                cmd.Parameters.AddWithValue("@StateN",ddState.SelectedValue);
                cmd.Parameters.AddWithValue("@CityN",ddCity.SelectedValue);
                cmd.Parameters.AddWithValue("@CscName",txtbox.Text);
                if(con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
                Response.Write("<script>alert('inserted successfully..')</script>");
                if(con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
    }
}