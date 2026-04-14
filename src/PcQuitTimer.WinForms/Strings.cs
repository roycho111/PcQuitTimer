using System;
using System.Globalization;

namespace PcQuitTimer;

public static class Strings
{
    private static bool? _forceKorean;

    public static bool IsKorean => _forceKorean ?? CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ko";

    public static event Action LanguageChanged;

    public static void SetKorean(bool korean)
    {
        _forceKorean = korean;
        LanguageChanged?.Invoke();
    }

    // Tabs
    public static string TabTimer => IsKorean ? "타이머" : "Timer";
    public static string TabSchedule => IsKorean ? "반복 예약" : "Schedule";

    // Timer mode
    public static string Countdown => IsKorean ? "카운트다운" : "Countdown";
    public static string SpecificTime => IsKorean ? "날짜/시간 지정" : "Date/Time";

    // Time units
    public static string Hours => IsKorean ? "시간" : "h";
    public static string Minutes => IsKorean ? "분" : "m";
    public static string Seconds => IsKorean ? "초" : "s";

    // Labels
    public static string Action => IsKorean ? "동작:" : "Action:";
    public static string Time => IsKorean ? "시간:" : "Time:";

    // Buttons
    public static string Start => IsKorean ? "시작" : "Start";
    public static string Stop => IsKorean ? "중지" : "Stop";
    public static string Add => IsKorean ? "추가" : "Add";
    public static string Toggle => IsKorean ? "켜기/끄기" : "On/Off";
    public static string Remove => IsKorean ? "삭제" : "Remove";
    public static string LangToggle => IsKorean ? "EN" : "한";

    // Messages
    public static string MsgEnterTime => IsKorean ? "시간을 입력하세요." : "Please enter a time.";
    public static string MsgFutureTime => IsKorean ? "미래 시간을 선택하세요." : "Please select a future time.";
    public static string MsgSelectDay => IsKorean ? "요일을 선택하세요." : "Please select at least one day.";
    public static string MsgNotice => IsKorean ? "알림" : "Notice";

    // Days
    public static string Mon => IsKorean ? "월" : "Mon";
    public static string Tue => IsKorean ? "화" : "Tue";
    public static string Wed => IsKorean ? "수" : "Wed";
    public static string Thu => IsKorean ? "목" : "Thu";
    public static string Fri => IsKorean ? "금" : "Fri";
    public static string Sat => IsKorean ? "토" : "Sat";
    public static string Sun => IsKorean ? "일" : "Sun";
    public static string Everyday => IsKorean ? "매일" : "Everyday";

    // Power actions
    public static string GetActionName(PowerAction action) => action switch
    {
        PowerAction.Shutdown => IsKorean ? "종료" : "Shutdown",
        PowerAction.Restart => IsKorean ? "재시작" : "Restart",
        PowerAction.Sleep => IsKorean ? "절전" : "Sleep",
        PowerAction.Hibernate => IsKorean ? "최대 절전" : "Hibernate",
        PowerAction.LogOff => IsKorean ? "로그오프" : "Log off",
        PowerAction.Lock => IsKorean ? "잠금" : "Lock",
        _ => action.ToString()
    };

    public static string GetDayName(DayOfWeek day) => day switch
    {
        DayOfWeek.Monday => Mon,
        DayOfWeek.Tuesday => Tue,
        DayOfWeek.Wednesday => Wed,
        DayOfWeek.Thursday => Thu,
        DayOfWeek.Friday => Fri,
        DayOfWeek.Saturday => Sat,
        DayOfWeek.Sunday => Sun,
        _ => ""
    };
}
