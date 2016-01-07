﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Umeng;
using UnityEngine.Advertisements;
using System;
using GoogleMobileAds.Api;



public class ads_controller  {



	public int unity_ads_state = 0;
//	public int youmi_ads_state = 0;
	public int admob_ads_state = 0;

	public int  cur_ads = 0; //0:unity 1:admob 2:youmi 3:youmi_video
	public int cur_reward_ads = 0;//0:unity 1:admob 2:youmi 



	AndroidJavaClass mJc;
	AndroidJavaObject mJo;
	static ads_controller cont = null;

	InterstitialAd nor_admob = null,reward_admob=null;

	public static string deviceid = "";

	[HideInInspector]
//	public int youmi_video_state = 0;

	public static ads_controller share_ads(){
		if(cont == null){
			cont = new ads_controller();

		}
		return cont;
	}


	public void Destroy_share(){
		nor_admob.Destroy ();
		reward_admob.Destroy ();

	}


	public string getDeviceID(){
		string devicdID = "";
#if UNITY_ANDROID
		devicdID = mJo.Call<string>("getDeviceID");
#elif UNITY_IOS

#endif
		return devicdID;
	}



	public string getSD(){

		string sd = "";
#if UNITY_ANDROID
		sd = mJo.Call<string>("getSD");
#elif UNITY_IOS

#endif
		return sd;

	}


	public string getQD(){

		string qd = "";
#if UNITY_ANDROID
		qd = mJo.Call<string>("getYouMengQuDao");
#elif UNITY_IOS

#endif
		return qd;

	}




	public  ads_controller(){
		#if UNITY_ANDROID
		mJc=new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		mJo=mJc.GetStatic<AndroidJavaObject>("currentActivity");
//		this.youmi_video_state = mJo.Call<int>("getVideostate");

//		string nor_adId = "ca-app-pub-3940256099942544/1033173712";
//		string reward_adId = "ca-app-pub-3940256099942544/1033173712";

		string nor_adId = "ca-app-pub-6426348923391842/6822076817";
		string reward_adId = "ca-app-pub-6426348923391842/2252276414";





#elif UNITY_IOS
		string adUnitId = "";
		string reward_adId = "";
	#endif


		Debug.Log("ads1");
		// Initialize an InterstitialAd.
		if(nor_admob==null){
			nor_admob = new InterstitialAd(nor_adId);
			request_nor_admob(null,null);
			nor_admob.AdClosed+=nor_admob_closed;
			nor_admob.AdFailedToLoad+=request_nor_admob;
		}

		if(reward_admob == null){
			reward_admob = new InterstitialAd(reward_adId);

			Debug.Log("ads2");
			// Create an empty ad request.


			request_reward_admob(null,null);


			reward_admob.AdClosed+=reward_admob_closed;
			reward_admob.AdFailedToLoad +=request_reward_admob;

			Debug.Log("ads4");
		}
	}







	public void request_nor_admob(object sender, EventArgs args){

//		AdRequest request = new AdRequest.Builder().AddTestDevice(deviceid).Build();
		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		nor_admob.LoadAd(request);

	}

	public void request_reward_admob(object sender, EventArgs args){


//		AdRequest request_1 = new AdRequest.Builder().AddTestDevice(deviceid).Build();
		AdRequest request_1 = new AdRequest.Builder().Build();
		reward_admob.LoadAd(request_1);


	}

	public void nor_admob_closed(object sender, EventArgs args){
		request_nor_admob(sender,args);

	}

	public void reward_admob_closed(object sender, EventArgs args){

		reward("1");
		request_reward_admob(sender,args);

	}

	public void Start(){

	}

	void Update(){


	}








	

	public int get_reward_ads_state(){
		int state = 0; //0:no ads,1:have ads
#if UNITY_IOS
		state = get_unity_ads(1);
#elif UNITY_ANDROID
		unity_ads_state = get_unity_ads(1);

		admob_ads_state = get_admob_state(1);

		state = unity_ads_state+admob_ads_state;
		Debug.Log("UNITY ADS state is "+unity_ads_state+",admob state is"+admob_ads_state);

#endif
		return state;

	}


	int get_youmi_state(int type){
		int state = 0;

		state = mJo.Call<int>("getVideostate");
		return state;

	}

	int get_admob_state(int type){

		if(type == 0){

			if(nor_admob.IsLoaded()){
				return 1;
			}
			return 0;

		}else{
			if(reward_admob.IsLoaded()){

				return 1;

			}
			return 0;

		}
	}


		
	int get_unity_ads(int type){
		
		if(type == 0){
			
			if(Advertisement.IsReady()){
				return 1;
			}
			return 0;
		}else{
			if(Advertisement.IsReady("rewardedVideo")){
			
				return 1;
			}
			return 0;
		}
	}


	public void show_nor_ads(float ads_yz){
		float t = UnityEngine.Random.value;
		if(t>ads_yz){

			return;
		}

		GA.Event(tongji.ADS,tongji.NOR_ADS);
#if UNITY_IOS

		show_nor_unity_ads();
#elif UNITY_ANDROID


		cur_ads = (cur_ads+1)%4;
		if(cur_ads == 0){
			if(Advertisement.IsReady()){
				
				
				show_nor_unity_ads();
			}
		}else if(cur_ads == 1){
			if(nor_admob.IsLoaded()){
				
				show_nor_admob();
			}
		}else if(cur_ads == 2){
//			if(get_youmi_state(0) == 1){
//				show_nor_youmi_ads(0);
//			}

		}else {

//			show_nor_youmi_ads(1);
		}




//		show_nor_admob();
#endif

	}






	public void show_nor_admob(){
		Dictionary<string,string> dic = new Dictionary<string,string>();
		dic[tongji.NOR_ADS] = tongji.ADMOB_ADS;
		GA.Event(tongji.ADS,dic);
		nor_admob.Show();

	}



	public void show_nor_unity_ads(){
		Dictionary<string,string> dic = new Dictionary<string,string>();
		dic[tongji.NOR_ADS] = tongji.UNITY_ADS;
		GA.Event(tongji.ADS,dic);
		Advertisement.Show();

	}


	public void show_nor_youmi_ads(int i){
		Dictionary<string,string> dic = new Dictionary<string,string>();
		dic[tongji.NOR_ADS] = tongji.YOUMI_ADS;
		GA.Event(tongji.ADS,dic);
		if( i == 1){
			mJo.Call("showSpot","-1");
		}else{

			mJo.Call("showVideo","-1");
		}
	}

	public  void  show_reward_ads(){
		GA.Event(tongji.ADS,tongji.NEXT_GATE_ADS);
#if UNITY_IOS
		show_reward_unity_ads();
#elif UNITY_ANDROID
		

	if(admob_ads_state == 1){
			
			show_reward_admob();
		}else if(unity_ads_state == 1){
			
			show_reward_unity_ads();
		}
//		else if(youmi_ads_state == 1){
//			show_reward_youmi_ads();
//		}
//		show_reward_youmi_ads();
#endif


		
	}

	void show_reward_youmi_ads(){
		Dictionary<string,string> dic = new Dictionary<string,string>();
		dic[tongji.NEXT_GATE_ADS] = tongji.YOUMI_ADS;
		GA.Event(tongji.ADS,dic);


	}

	void  show_reward_unity_ads(){
		Dictionary<string,string> dic = new Dictionary<string,string>();
		dic[tongji.NEXT_GATE_ADS] = tongji.UNITY_ADS;
		GA.Event(tongji.ADS,dic);
		if(Advertisement.IsReady("rewardedVideo")){
			UnityEngine.Advertisements.ShowOptions op = new ShowOptions();
			op.resultCallback = result_suc;


			Advertisement.Show("rewardedVideo",op);
		}else {
			Debug.Log("tangjie noready play ads");

		}
	}

	public void show_reward_admob(){
		Dictionary<string,string> dic = new Dictionary<string,string>();
		dic[tongji.NEXT_GATE_ADS] = tongji.ADMOB_ADS;
		GA.Event(tongji.ADS,dic);

		reward_admob.Show();

	}






	public void reward_result(string  s_result){
		int result = int.Parse(s_result);
		switch(result){
		case 1:

			Dictionary<string,string > dic = new Dictionary<string,string>();
			dic[tongji.ADS_SUC] = tongji.YOUMI_ADS;
			GA.Event(tongji.ADS,dic);

			break;
		case 0:
			Dictionary<string,string> f_dic = new Dictionary<string,string>();
			f_dic[tongji.ADS_FAILED] = tongji.YOUMI_ADS;
			GA.Event(tongji.ADS,f_dic);
			Debug.Log("ads failed");
			break;
		case -1:
			Debug.Log("ads skipped");
			break;
		}
		reward(s_result);

	}

	public void reward(string s_result){
		int result = int.Parse(s_result);
		switch(result){
		case 1:


			GA.Event(tongji.GATE_ADS,Application.loadedLevelName);
			
			
			map_data_manager.add_max_num();
			if(map_data_manager.cur_num != map_data_manager.max_num)
				map_data_manager.cur_num++;
			gate_common.next_gate = "gate_"+map_data_manager.cur_num;
			Application.LoadLevel("loading_scence");
			break;
		case 0:
			Debug.Log("ads failed");
			break;
		case -1:
			Debug.Log("ads skipped");
			break;
		}

	}

	public	void result_suc(ShowResult result){
		int r = 0;
		switch(result){
		case ShowResult.Finished :
			r = 1;
			Dictionary<string,string> dic = new Dictionary<string,string>();
			dic[tongji.ADS_SUC] = tongji.UNITY_ADS;
			GA.Event(tongji.ADS,dic);
			break;
		case ShowResult.Failed:
			Dictionary<string,string> f_dic = new Dictionary<string,string>();
			f_dic[tongji.ADS_FAILED] = tongji.UNITY_ADS;
			GA.Event(tongji.ADS,f_dic);
			r = 0;
			break;
		case ShowResult.Skipped:
			r = -1;
			break;
		}
		reward(""+r);
	}
	

	public void quite_app(string s){
		Destroy_share ();
		mJo.Call("quiteApp",s);
	}

}
