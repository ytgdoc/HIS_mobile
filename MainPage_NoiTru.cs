using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using System.Data;
using System.Linq.Expressions;
using Android.Graphics;

namespace HIS
{
    [Activity(Label = "Nội Trú", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape, Theme = "@style/ThemeHis", Icon = "@drawable/HealthCare")]
    public class MainPage_NoiTru : Activity, Android.Widget.TabHost.IOnTabChangeListener
    {
         TabHost tab;
         private string s_UserName="";
         private TextView txtView;
         private Spinner dm_khoaphong;
         private bool flag=false;
         private MedilinkWS.MedilinkService service;
         private DataTable tb_khoaPhong,ds_hiendien;
         private ArrayAdapter<String> SpinerAdapter = null;
         private int count=0;
         private int count_dshd = 0;
         private TableLayout tl;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.MainPage_NoiTru);
            s_UserName = Intent.GetStringExtra("Username") ?? "Tên đăng nhập không hợp lệ";
            //Load Content Data
            LoadTab();
            LoadWidget();
            service = new MedilinkWS.MedilinkService();
            service.List_IDDepartmentsOfUserCompleted += service_List_IDDepartmentsOfUserCompleted;
            service.List_IDDepartmentsOfUserAsync(s_UserName);

            //Call_WS();
           // LoadMethod();
            //Toast.MakeText(this, s_UserName, ToastLength.Short).Show();
        }
        private void Call_WS(){
          service = new MedilinkWS.MedilinkService();
        }
        private void LoadMethod() {
            service.List_IDDepartmentsOfUserCompleted += service_List_IDDepartmentsOfUserCompleted;
            service.List_IDDepartmentsOfUserAsync(s_UserName);
        }
        private void LoadWidget()
        {
            txtView = FindViewById<TextView>(Resource.Id.txtView);
            dm_khoaphong = FindViewById<Spinner>(Resource.Id.dm_khoaphong);
            //List_benhnhan = FindViewById<ListView>(Resource.Id.List_benhnhan);
             tl = FindViewById<TableLayout>(Resource.Id.ds_benhnhan);
        }

        void service_List_IDDepartmentsOfUserCompleted(object sender, MedilinkWS.List_IDDepartmentsOfUserCompletedEventArgs e)
        {
                service.List_DepartmentsOfUserCompleted +=service_List_DepartmentsOfUserCompleted;
                service.List_DepartmentsOfUserAsync(e.Result, s_UserName);
        }

        void service_List_DepartmentsOfUserCompleted(object sender, MedilinkWS.List_DepartmentsOfUserCompletedEventArgs e)
        {
           tb_khoaPhong = e.Result;
           count = tb_khoaPhong.Rows.Count;
            string[] items =new string[count];
           try
           {
               if (count > 0)
               {
                  // name = tb_khoaPhong.Columns[1].ToString();
                   for (int i = 0; i < count; i++)
                   {
                       items[i] = tb_khoaPhong.Rows[i][1].ToString();
                   }
                   SpinerAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleSpinnerItem, items);
                   //phải gọi lệnh này để hiển thị danh sách cho Spinner
                   SpinerAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleListItemSingleChoice);
                   //Thiết lập adapter cho Spinner
                   dm_khoaphong.Adapter = SpinerAdapter;
                   //thiet lap su kien cho tung item trong Spinner
                   dm_khoaphong.ItemSelected += dm_khoaphong_ItemSelected;
               }
               else {
                   Toast.MakeText(this, "Bạn chưa được phân quyền vào các khoa nội trú",ToastLength.Long).Show();
               }
              
           }
           catch (Exception ex)
           {
               string s = ex.ToString();
           }
          

        }

        void dm_khoaphong_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            string Id_KhoaPhongNT = "";
            count = tb_khoaPhong.Rows.Count;
            int j = 0;
            if (e.Position != 0)
            {
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (e.Position.Equals(tb_khoaPhong.Rows[i][1].ToString()))
                        {
                            j = i;
                            break;
                        }

                    }
                    Id_KhoaPhongNT = tb_khoaPhong.Rows[j][2].ToString();
                    service.ListPatientOfDepartmentCompleted += service_ListPatientOfDepartmentCompleted;
                    service.ListPatientOfDepartmentAsync(Id_KhoaPhongNT);
                    flag = true;
                }
            }
            else {
                Id_KhoaPhongNT = tb_khoaPhong.Rows[e.Position][2].ToString();
                service.ListPatientOfDepartmentCompleted += service_ListPatientOfDepartmentCompleted;
                service.ListPatientOfDepartmentAsync(Id_KhoaPhongNT);
                flag = false;
            }
           
        }

        void service_ListPatientOfDepartmentCompleted(object sender, MedilinkWS.ListPatientOfDepartmentCompletedEventArgs e)
        {
            ds_hiendien = e.Result;
            int row_Id = 0;
            int TextView_Id = 1;
            int SoTT = 1;
            //int Auto_Id = 1;
            //string ma_Id = "txtMabn" + Auto_Id.ToString();
            //string ten_Id = "txtTen" + Auto_Id.ToString();
            //string ngayvv_Id = "txtNgayVv" + Auto_Id.ToString();
            //string gioitinh_Id = "txtGioiTinh" + Auto_Id.ToString();
            //string namsinh_Id = "txtNamSinh" + Auto_Id.ToString();

            count_dshd = ds_hiendien.Rows.Count;

            string[] mabn = new string[count_dshd];
            string[] tenbn = new string[count_dshd];
            DateTime[] ngayvv = new DateTime[count_dshd];
            int[] gioitinh = new int[count_dshd];
            int[] namsinh = new int[count_dshd];
            string[] benhchinh = new string[count_dshd];
            //decimal[] maql = new decimal[count_dshd];

            try
            {
                if (count_dshd > 0)
                {
                    for (int i = 0; i < count_dshd; i++)
                    {
                        row_Id++;
                        mabn[i] = ds_hiendien.Rows[i]["ma"].ToString();
                        tenbn[i] = ds_hiendien.Rows[i]["ten"].ToString();
                        ngayvv[i] = DateTime.Parse(ds_hiendien.Rows[i]["ngayvaovien"].ToString());
                        gioitinh[i] =Int32.Parse(ds_hiendien.Rows[i]["phai"].ToString());
                        namsinh[i] =Int32.Parse(ds_hiendien.Rows[i]["namsinh"].ToString());
                        benhchinh[i] = ds_hiendien.Rows[i]["benhchinh"].ToString();
                       // maql[i] = decimal.Parse(ds_hiendien.Rows[i]["maql"].ToString());
                        if (flag)
                        {
                            tl.RemoveAllViews();
                            TableRow tbr = new TableRow(this);
                            tbr.Id = row_Id;
                            TableRow.LayoutParams layoutParams = new TableRow.LayoutParams(TableRow.LayoutParams.MatchParent,
                                TableRow.LayoutParams.WrapContent);
                            var layoutParamTV = new TableLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                  ViewGroup.LayoutParams.WrapContent) { LeftMargin = 3 };
                            tbr.LayoutParameters = layoutParams;

                            TextView tv_SoTT = new TextView(this);
                            //tv_Ma.Id =Int32.Parse(ma_Id);
                            tv_SoTT.Id = TextView_Id;
                            tv_SoTT.Text = SoTT.ToString();
                            tv_SoTT.SetTextColor(Android.Graphics.Color.Blue);
                            //tv_Ma.LayoutParameters = layoutParamTV;
                            tbr.AddView(tv_SoTT);
                            SoTT++;
                            TextView_Id++;

                            TextView tv_Ma = new TextView(this);
                            //tv_Ma.Id =Int32.Parse(ma_Id);
                            tv_Ma.Id = TextView_Id;
                            tv_Ma.Text = mabn[i];
                            tv_Ma.SetTextColor(Android.Graphics.Color.Blue);
                            //tv_Ma.LayoutParameters = layoutParamTV;
                            tbr.AddView(tv_Ma);
                            TextView_Id++;

                            TextView TV_Tenbn = new TextView(this);
                            // TV_Tenbn.Id = Int32.Parse(ten_Id);
                            TV_Tenbn.Id = TextView_Id;
                            TV_Tenbn.Text = tenbn[i];
                            TV_Tenbn.SetTextColor(Android.Graphics.Color.Blue);
                            //TV_Tenbn.LayoutParameters = layoutParamTV;
                            tbr.AddView(TV_Tenbn);
                            TextView_Id++;

                            TextView TV_NgayVv = new TextView(this);
                            //TV_NgayVv.Id = Int32.Parse(ngayvv_Id);
                            TV_NgayVv.Id = TextView_Id;
                            TV_NgayVv.Text = ngayvv[i].ToString();
                            TV_NgayVv.SetTextColor(Android.Graphics.Color.Blue);
                            //TV_Tenbn.LayoutParameters = layoutParamTV;
                            tbr.AddView(TV_NgayVv);
                            TextView_Id++;

                            TextView TV_GioiTinh = new TextView(this);
                            //TV_GioiTinh.Id = Int32.Parse(gioitinh_Id);
                            TV_GioiTinh.Id = TextView_Id;
                            TV_GioiTinh.Text = gioitinh[i].ToString();
                            TV_GioiTinh.SetTextColor(Android.Graphics.Color.Blue);
                            //TV_Tenbn.LayoutParameters = layoutParamTV;
                            tbr.AddView(TV_GioiTinh);
                            TextView_Id++;

                            TextView TV_NamSinh = new TextView(this);
                            //TV_NamSinh.Id = Int32.Parse(namsinh_Id);
                            TV_NamSinh.Id = TextView_Id;
                            TV_NamSinh.Text = namsinh[i].ToString();
                            TV_NamSinh.SetTextColor(Android.Graphics.Color.Blue);
                            //TV_Tenbn.LayoutParameters = layoutParamTV;
                            tbr.AddView(TV_NamSinh);
                            TextView_Id++;

                            TextView TV_BenhChinh = new TextView(this);
                            //TV_NamSinh.Id = Int32.Parse(namsinh_Id);
                            TV_BenhChinh.Id = TextView_Id;
                            TV_BenhChinh.Text = benhchinh[i].ToString();
                            TV_BenhChinh.SetTextColor(Android.Graphics.Color.Blue);
                            //TV_Tenbn.LayoutParameters = layoutParamTV;
                            tbr.AddView(TV_BenhChinh);
                            TextView_Id++;
                            //Auto_Id++;
                            // Add the TableRow to the TableLayout
                            tl.AddView(tbr);
                        }
                        else {
                            TableRow tbr = new TableRow(this);
                            tbr.Id = row_Id;
                            TableRow.LayoutParams layoutParams = new TableRow.LayoutParams(TableRow.LayoutParams.MatchParent,
                                TableRow.LayoutParams.WrapContent);
                            var layoutParamTV = new TableLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                  ViewGroup.LayoutParams.WrapContent) { LeftMargin = 3 };
                            tbr.LayoutParameters = layoutParams;

                            TextView tv_SoTT = new TextView(this);
                            //tv_Ma.Id =Int32.Parse(ma_Id);
                            tv_SoTT.Id = TextView_Id;
                            tv_SoTT.Text = SoTT.ToString();
                            tv_SoTT.SetTextColor(Android.Graphics.Color.Blue);
                            //tv_Ma.LayoutParameters = layoutParamTV;
                            tbr.AddView(tv_SoTT);
                            SoTT++;
                            TextView_Id++;

                            TextView tv_Ma = new TextView(this);
                            //tv_Ma.Id =Int32.Parse(ma_Id);
                            tv_Ma.Id = TextView_Id;
                            tv_Ma.Text = mabn[i];
                            tv_Ma.SetTextColor(Android.Graphics.Color.Blue);
                            //tv_Ma.LayoutParameters = layoutParamTV;
                            tbr.AddView(tv_Ma);
                            TextView_Id++;

                            TextView TV_Tenbn = new TextView(this);
                            // TV_Tenbn.Id = Int32.Parse(ten_Id);
                            TV_Tenbn.Id = TextView_Id;
                            TV_Tenbn.Text = tenbn[i];
                            TV_Tenbn.SetTextColor(Android.Graphics.Color.Blue);
                            //TV_Tenbn.LayoutParameters = layoutParamTV;
                            tbr.AddView(TV_Tenbn);
                            TextView_Id++;

                            TextView TV_NgayVv = new TextView(this);
                            //TV_NgayVv.Id = Int32.Parse(ngayvv_Id);
                            TV_NgayVv.Id = TextView_Id;
                            TV_NgayVv.Text = ngayvv[i].ToString();
                            TV_NgayVv.SetTextColor(Android.Graphics.Color.Blue);
                            //TV_Tenbn.LayoutParameters = layoutParamTV;
                            tbr.AddView(TV_NgayVv);
                            TextView_Id++;

                            TextView TV_GioiTinh = new TextView(this);
                            //TV_GioiTinh.Id = Int32.Parse(gioitinh_Id);
                            TV_GioiTinh.Id = TextView_Id;
                            TV_GioiTinh.Text = gioitinh[i].ToString();
                            TV_GioiTinh.SetTextColor(Android.Graphics.Color.Blue);
                            //TV_Tenbn.LayoutParameters = layoutParamTV;
                            tbr.AddView(TV_GioiTinh);
                            TextView_Id++;

                            TextView TV_NamSinh = new TextView(this);
                            //TV_NamSinh.Id = Int32.Parse(namsinh_Id);
                            TV_NamSinh.Id = TextView_Id;
                            TV_NamSinh.Text = namsinh[i].ToString();
                            TV_NamSinh.SetTextColor(Android.Graphics.Color.Blue);
                            //TV_Tenbn.LayoutParameters = layoutParamTV;
                            tbr.AddView(TV_NamSinh);
                            TextView_Id++;
                            //Auto_Id++;

                            TextView TV_BenhChinh = new TextView(this);
                            //TV_NamSinh.Id = Int32.Parse(namsinh_Id);
                            TV_BenhChinh.Id = TextView_Id;
                            TV_BenhChinh.Text = benhchinh[i].ToString();
                            TV_BenhChinh.SetTextColor(Android.Graphics.Color.Blue);
                            //TV_Tenbn.LayoutParameters = layoutParamTV;
                            tbr.AddView(TV_BenhChinh);
                            TextView_Id++;


                            // Add the TableRow to the TableLayout
                            tl.AddView(tbr);
                        }
                    }
                    
                }
                else
                {
                    Toast.MakeText(this, "Không có bệnh nhân nào trong khoa phòng nay.", ToastLength.Short).Show();
                }
            }
            catch (Exception ex) {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
            }

            service.CloseConnection();
            service.CloseConnectionAsync();
            

        }

       
        //Cau hinh tab
        public void LoadTab() { 
            //Lấy Tabhost id ra trước (cái này của built - in android
                tab = FindViewById<TabHost>(Android.Resource.Id.TabHost);
            //goi lenh setup
                tab.Setup();
                TabHost.TabSpec spec;
            //Tao tab Noi Tru
                spec = tab.NewTabSpec("Nội Trú");
                spec.SetContent(Resource.Id.Noitru);
                spec.SetIndicator("Nội Trú");
                tab.AddTab(spec);
                //Tạo Phong khám
                spec = tab.NewTabSpec("Phòng Khám");
                spec.SetContent(Resource.Id.Phongkham);
                spec.SetIndicator("Phòng Khám");
                tab.AddTab(spec);
                //Thiết lập tab mặc định được chọn ban đầu là tab 0
                tab.SetCurrentTabByTag("noitru");
                tab.SetOnTabChangedListener(this);
        }

        public void OnTabChanged(string tabId)
        {
            string s = "Bạn đã chọn " + tabId + "  ";
            Toast.MakeText(this, s, ToastLength.Short).Show();
        }
    }
}