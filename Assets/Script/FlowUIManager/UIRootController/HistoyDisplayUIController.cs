using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Kun.Data;

namespace Kun.Tool
{
	public class HistoyDisplayUIController : UIRootController
	{
		public HistoyDisplayUIController (RefBinder refBinder, SpeedSettingData speedSettingData) : base (refBinder.gameObject)
		{
			progressTweenController = refBinder.GetComponent<PosTweenController> (AssetKeys.ProgressValuePosController);
			speedDropdown = refBinder.GetComponent<Dropdown> (AssetKeys.SpeedScaleDropDown);

			speeds = new List<float> (speedSettingData.Speeds);
			defaultIndex = speedSettingData.DefaultIndex;

			List<string> speedDropdownSelections = new List<string> ();

			speeds.ForEach (speed=> 
			{
				speedDropdownSelections.Add (speed.ToString ());
			});

			speedDropdown.AddOptions (speedDropdownSelections);
			speedDropdown.onValueChanged.AddListener (OnSpeedDropdownChanege);
		}

		List<float> speeds;
		int defaultIndex;

		Dropdown speedDropdown;
		PosTweenController progressTweenController;
		event Action<float> onSpeedChangeEvent;

		public void SetDefaultSpeed ()
		{
			speedDropdown.value = defaultIndex;
			onSpeedChangeEvent.Invoke (speeds[defaultIndex]);
		}

		void OnSpeedDropdownChanege (int value)
		{
			float newSpeed = speeds[value];
			onSpeedChangeEvent.Invoke (newSpeed);
		}

		public void RegeistSpeedChangeEvent (Action<float> callback)
		{
			onSpeedChangeEvent += callback;
		}

		public void RemoveSpeedChangeEvent (Action<float> callback)
		{
			onSpeedChangeEvent -= callback;
		}

		public void SetProgress (float progress)
		{
			progressTweenController.SetValue (progress);
		}

		public override void SwitchStatus (GameFlowUIStatus status)
		{
			base.SwitchStatus (status);

			//開始玩 才能重置
			switch (status) 
			{
			case GameFlowUIStatus.GameStart:
				{
					bindGO.SetActive (false);
					break;
				}

			case GameFlowUIStatus.Standby:
				{
					bindGO.SetActive (false);
					break;
				}

            case GameFlowUIStatus.History:
                {
                    bindGO.SetActive (true);
                    break;
                }
			}
		}
	}
}