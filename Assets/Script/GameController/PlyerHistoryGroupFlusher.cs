﻿using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kun.Tool;
using Kun.Data;

namespace Kun.Controller
{
	public class PlyerHistoryGroupFlusher
	{	
		public PlyerHistoryGroupFlusher (Action<PlayHistoryGroup> onFlushCallback)
		{
			waitProcessLocker = new object ();
			waitFlusherLocker = new object ();
			waitProcessDatas = new List<PlayHistoryGroup> ();
			onFlushEvent = onFlushCallback;
		}

		public void Run ()
		{
			runThread = new Thread (CheckFlusher);
			runThread.Start ();
		}

		public void AddPlayHistoryGroup (PlayHistoryGroup data)
		{
			lock (waitProcessLocker) 
			{
				waitProcessDatas.Add (data);
			}
		}

		event Action<PlayHistoryGroup> onFlushEvent;

		object waitProcessLocker;

		object waitFlusherLocker;

		List<PlayHistoryGroup> waitProcessDatas = new List<PlayHistoryGroup> ();

		Thread runThread;

		void CheckFlusher ()
		{
			while (true) 
			{
				List<PlayHistoryGroup> _waitProcessDatas = new List<PlayHistoryGroup> ();

				lock (waitProcessLocker) 
				{
					try
					{
						_waitProcessDatas = new List<PlayHistoryGroup> (waitProcessDatas);
						waitProcessDatas.Clear ();
					}
					catch(Exception e) 
					{
						Debug.LogError (e.Message);
					}
				}

				if (_waitProcessDatas.Count == 0) 
				{
					SpinWait.SpinUntil (()=>
						{
							return (waitProcessDatas.Count > 0);
						});
				}
				else
				{
					_waitProcessDatas.ForEach (data=>
						{
							Flush (data);
						});
				}
			}
		}

		void Flush (PlayHistoryGroup data)
		{
			lock (waitFlusherLocker) 
			{
				onFlushEvent.Invoke (data);
			}
		}

		// 透過Locker確保存檔完畢
		void WaitFlushFinish ()
		{
			lock (waitFlusherLocker) 
			{
				
			}
		}


		public void Close ()
		{
			WaitFlushFinish ();
			
			if (runThread != null)
			{
				runThread.Abort ();
			}
		}
	}
}