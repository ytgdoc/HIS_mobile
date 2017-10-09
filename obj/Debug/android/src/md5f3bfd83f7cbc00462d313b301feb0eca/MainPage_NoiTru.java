package md5f3bfd83f7cbc00462d313b301feb0eca;


public class MainPage_NoiTru
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer,
		android.widget.TabHost.OnTabChangeListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onTabChanged:(Ljava/lang/String;)V:GetOnTabChanged_Ljava_lang_String_Handler:Android.Widget.TabHost/IOnTabChangeListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("HIS.MainPage_NoiTru, HIS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MainPage_NoiTru.class, __md_methods);
	}


	public MainPage_NoiTru () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MainPage_NoiTru.class)
			mono.android.TypeManager.Activate ("HIS.MainPage_NoiTru, HIS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onTabChanged (java.lang.String p0)
	{
		n_onTabChanged (p0);
	}

	private native void n_onTabChanged (java.lang.String p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
