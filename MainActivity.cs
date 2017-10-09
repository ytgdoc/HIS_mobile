using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Xml;
using Android.Content.PM;
using Android.Graphics;
using Android.Util;
using System.Threading;
using Android.Net;
using Java.Net;
using System.IO;
using System.Net.NetworkInformation;


namespace HIS
{
    [Activity(Label = "HIS", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/ThemeHis", Icon = "@drawable/HealthCare")]
    public class MainActivity : Activity, View.IOnLongClickListener
    {
        private Button btnOk;
        private EditText editUser, editPass;
        private TextView lblMsg, txtView;
        private TableLayout tb1, tb3;
        private Typeface typeface;
        private ProgressDialog progress;
        private string sPathWebservice = "";
        //check status internet
        private ConnectivityManager connectivityManager;
        bool connected = false;
        bool TestHost = false;
        bool TestClickBtn = false;
        private MedilinkWS.MedilinkService service;
        private string url,dataUser;
        private PingReply pingreply;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            this.sPathWebservice = this.ReadXML();

            //GetWitdget form xml layout
            editUser = FindViewById<EditText>(Resource.Id.editUser);
            editPass = FindViewById<EditText>(Resource.Id.editPass);
            btnOk = FindViewById<Button>(Resource.Id.btnOk);
            lblMsg = FindViewById<TextView>(Resource.Id.lblMSg);
            txtView = FindViewById<TextView>(Resource.Id.txtView);
            tb1 = FindViewById<TableLayout>(Resource.Id.tb1);
            tb3 = FindViewById<TableLayout>(Resource.Id.tb3);
            typeface = Typeface.CreateFromAsset(Assets, "font/times.ttf");
            txtView.SetTypeface(typeface, TypefaceStyle.Normal);
            btnOk.SetTypeface(typeface, TypefaceStyle.Normal);
            service = new MedilinkWS.MedilinkService();
            if (editPass.Text == string.Empty || editUser.Text == string.Empty)
            {
                btnOk.Enabled = false;
            }
            editUser.AfterTextChanged += editUser_AfterTextChanged;
            editPass.AfterTextChanged += editPass_AfterTextChanged;
            btnOk.Click += btnOk_Click;

            tb1.SetOnLongClickListener(this);
            tb3.Click += tb3_Click;
        }

        void tb3_Click(object sender, EventArgs e)
        {
            ExitHis();
        }
        void editPass_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            if (editPass.Text != string.Empty && editUser.Text != string.Empty)
            {
                //btnOk.Visibility = ViewStates.Invisible;
                btnOk.Enabled = true;
                btnOk.SetTypeface(typeface, TypefaceStyle.Bold);
                btnOk.SetBackgroundResource(Resource.Drawable.duongvienbuttonloginhover);
            }
            else
            {
                btnOk.Enabled = false;
                btnOk.SetBackgroundResource(Resource.Drawable.duongvienbuttonlogin);
            }
        }

        void editUser_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            if (editPass.Text != string.Empty && editUser.Text != string.Empty)
            {
                //btnOk.Visibility = ViewStates.Invisible;
                btnOk.Enabled = true;
                btnOk.SetTypeface(typeface, TypefaceStyle.Bold);
                btnOk.SetBackgroundResource(Resource.Drawable.duongvienbuttonloginhover);
            }
            else
            {
                btnOk.Enabled = false;
                btnOk.SetBackgroundResource(Resource.Drawable.duongvienbuttonlogin);
            }
        }

        string ReadXML()
        {
            try
            {
                string s_TenFile = "WebservicePath.xml";
                if (this.sPathWebservice == "")
                {
                    this.sPathWebservice = Android.OS.Environment.ExternalStorageDirectory.Path;
                    this.sPathWebservice = this.sPathWebservice.Trim('/');
                    this.sPathWebservice = this.sPathWebservice + "/dataHIS";
                    if (!System.IO.Directory.Exists(this.sPathWebservice))
                    {
                        System.IO.Directory.CreateDirectory(this.sPathWebservice);
                    }
                }
                string s_FileXML = this.sPathWebservice.Trim('/') + "//" + s_TenFile;
                if (!System.IO.File.Exists(s_FileXML))
                {
                    XmlWriterSettings objSetting = new XmlWriterSettings();
                    objSetting.Indent = true;
                    using (XmlWriter writer = XmlWriter.Create(s_FileXML, objSetting))
                    {
                        string ext = "version=\"1.0\" encoding=\"utf-8\"";
                        writer.WriteProcessingInstruction("xml", ext);
                        writer.WriteStartElement("config");
                        writer.WriteElementString("WebservicePath", "");
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                        writer.Flush();
                        writer.Close();
                    }
                    return "";
                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(s_FileXML);
                    return doc.GetElementsByTagName("WebservicePath").Item(0).InnerText;
                }
            }
            catch
            {
                return "";
            }
        }

        public bool hasActiveInternetConnection()
        {
            var connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);
            var activeConnection = connectivityManager.GetNetworkInfo(ConnectivityType.Wifi).GetState();
            //var activeConnection6 = connectivityManager.RequestRouteToHostAsync(ConnectivityType.Wifi,)
            if (activeConnection == NetworkInfo.State.Connected && activeConnection != null)
            {
                connected = true;
            }

            return connected;
        }
        private string PathStringConnection()
        {
            try
            {
                if (this.sPathWebservice != "") { service.Url = this.sPathWebservice; }
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
            return service.Url;
        }

        private bool hasActiveInternetConnectionHost
        {

            get
            {
                PathStringConnection();
                url = service.Url.Substring(7, 13);
                int timeout = 10000;
                Ping ping = new Ping();
                try
                {
                    pingreply = ping.Send(url, timeout);
                    if (pingreply.Status == IPStatus.Success)
                    {

                        TestHost = true;
                    }
                    else
                    {

                        TestHost = false;
                    }
                }
                catch
                {
                    TestHost = false;
                    throw new Exception();
                }
                return TestHost;
            }
        }


        void btnOk_Click(object sender, EventArgs e)
        {
            btnOk.SetBackgroundResource(Resource.Drawable.duongvienbuttonloginfocus);
            if (!hasActiveInternetConnection())
            {
                Toast.MakeText(this, "Không có kết nối mạng \n Vui lòng mở kết nối wifi", ToastLength.Long).Show();
                btnOk.SetBackgroundResource(Resource.Drawable.duongvienbuttonloginhover);
            } if (hasActiveInternetConnection() && !hasActiveInternetConnectionHost)
            {
                Toast.MakeText(this, "Có kết nối mạng nhưng không kết nối được máy chủ Vui lòng thử lại! ", ToastLength.Long).Show();
                btnOk.SetBackgroundResource(Resource.Drawable.duongvienbuttonloginhover);
            }

            else if (hasActiveInternetConnection() && hasActiveInternetConnectionHost)
            {

                PathStringConnection();

                progress = new ProgressDialog(this);
                progress.Indeterminate = true;
                progress.SetProgressStyle(ProgressDialogStyle.Spinner);

                progress.SetMessage("Đang kết nối máy chủ. Vui lòng chờ!...");
                progress.SetCancelable(false);
                progress.Show();

                service.bIsValidUserCompleted += service_bIsValidUserCompleted;
                service.bIsValidUserAsync(editUser.Text, editPass.Text);
            }

        }
        void service_bIsValidUserCompleted(object sender, MedilinkWS.bIsValidUserCompletedEventArgs e)
        {

            try
            {
                btnOk.Enabled = true;
                dataUser =editUser.Text.ToString();
                btnOk.SetBackgroundResource(Resource.Drawable.duongvienbuttonloginhover);
                if (e.Result)
                {
                    var noitruActivity = new Intent(this, typeof(MainPage_NoiTru));
                    noitruActivity.PutExtra("Username",dataUser);
                    StartActivity(noitruActivity);
                  // lblMsg.Text = "Đăng nhập thành công";
                    progress.Dismiss();
                }
                else
                {
                    lblMsg.Text = "Đăng nhập thất bại, Tên đăng nhập hoặc mật khẩu sai.";
                    progress.Dismiss();
                }

            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
        }

       

        public bool OnLongClick(View v)
        {
            ChooseOption();
            return false;
        }

        public void ExitHis()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Thoát chương trình");
            builder.SetMessage("Bạn có muốn thoát chương trình");
            builder.SetCancelable(false);
            builder.SetPositiveButton("OK", delegate
            {
                Finish();
            });
            builder.SetNegativeButton("Cancel", delegate { builder.Create().Cancel(); });
            builder.Create().Show();
        }
        public void ChooseOption()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Ping Server Host");
            builder.SetMessage("Do you to ping to server?");
            builder.SetCancelable(false);
            builder.SetPositiveButton("OK", (s, e) =>
            {
                if (!hasActiveInternetConnection())
                {
                    Toast.MakeText(this, "Không có kết nối mạng \n Vui lòng mở kết nối internet", ToastLength.Long).Show();
                }
                if (hasActiveInternetConnection() && !hasActiveInternetConnectionHost)
                {
                    progress = new ProgressDialog(this);
                    progress.Indeterminate = true;
                    progress.SetProgressStyle(ProgressDialogStyle.Spinner);
                    progress.SetTitle("Ping Server WS");
                    progress.SetMessage("Address: " + pingreply.Address + "\n" + "Tình trạng : TimeOut when connect Server! " + "\nThời gian kết nối  " + pingreply.RoundtripTime + " ms");
                    progress.SetCancelable(true);
                    progress.Show();
                }
                else if (hasActiveInternetConnection() && hasActiveInternetConnectionHost)
                {
                    progress = new ProgressDialog(this);
                    progress.Indeterminate = true;
                    progress.SetProgressStyle(ProgressDialogStyle.Spinner);
                    progress.SetTitle("Ping Server WS");
                    progress.SetMessage("Address:" + pingreply.Address + "\n" + "Tình trạng : " + pingreply.Status + "\nThời gian kết nối " + pingreply.RoundtripTime + " ms");
                    progress.SetCancelable(true);
                    progress.Show();
                }
            });
            builder.SetNegativeButton("Cancel", delegate { builder.Create().Cancel(); });
            builder.Create().Show();
        }

    }
}

