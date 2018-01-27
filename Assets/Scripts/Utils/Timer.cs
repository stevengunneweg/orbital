using UnityEngine;

public struct Timer
{
	private float 	startTime,
	duration;
	
	public 	Timer(float duration)
	{
		startTime = Time.time;
		this.duration = duration;
	}
	
	private float	endTime {
		get {
			return startTime + duration;
		}
	}
	
	public 	float	progress {
		get {
			return time / duration;
		}
	}
	
	public	float	remaining {
		get {
			return (endTime - Time.time);
		}
	}
	
	public	float	time {
		get {
			return (Time.time - startTime);
		}
	}
	
	public override string ToString()
	{
		return "startTime: " + startTime + " duration: " + duration + " endTime: " + endTime + " progress: " + progress + " remaining: " + remaining;
	}
	
	public 	static implicit operator bool(Timer t)
	{
		return !(Time.time >= t.endTime);
	}
}
