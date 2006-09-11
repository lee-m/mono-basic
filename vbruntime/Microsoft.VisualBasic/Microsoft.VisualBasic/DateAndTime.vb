'
' DateAndTime.vb
'
' Author:
'   Chris J Breisch (cjbreisch@altavista.net) 
'   Pablo Cardona (pcardona37@hotmail.com) CRL Team
'   Mizrahi Rafael (rafim@mainsoft.com)
'

'
' Copyright (C) 2002-2006 Mainsoft Corporation.
' Copyright (C) 2004-2006 Novell, Inc (http://www.novell.com)
'
' Permission is hereby granted, free of charge, to any person obtaining
' a copy of this software and associated documentation files (the
' "Software"), to deal in the Software without restriction, including
' without limitation the rights to use, copy, modify, merge, publish,
' distribute, sublicense, and/or sell copies of the Software, and to
' permit persons to whom the Software is furnished to do so, subject to
' the following conditions:
' 
' The above copyright notice and this permission notice shall be
' included in all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
' EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
' MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
' NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
' LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
' OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
' WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
'

Imports System
Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.Globalization
Imports Microsoft.VisualBasic.CompilerServices

Namespace Microsoft.VisualBasic
    Public Module DateAndTime
        Public Property DateString() As String
            Get
                Return DateTime.Today.ToString("MM-dd-yyyy")
            End Get

            Set(ByVal Value As String)
                Dim formats() As String = {"M-d-yyyy", "M-d-y", "M/d/yyyy", "M/d/y"}

                Try
                    Dim dtToday As DateTime = DateTime.ParseExact(Value, formats, _
                     DateTimeFormatInfo.CurrentInfo, _
                     DateTimeStyles.None)

                    Today = dtToday
                Catch e As FormatException
                    Throw New InvalidCastException(String.Format("Cast from string {0} to type 'Date' is not valid.", Value))
                End Try
            End Set
        End Property
#If TARGET_JVM = False Then
        <DllImport("libc", EntryPoint:="stime", _
           SetLastError:=True, CharSet:=CharSet.Unicode, _
           ExactSpelling:=True, _
           CallingConvention:=CallingConvention.StdCall)> _
        Public Function stime(ByVal t As Integer) As Integer
            ' Leave function empty - DllImport attribute forwards calls to stime to
            ' stime in libc.dll
        End Function
#End If

        Public Property Today() As System.DateTime
            Get
                Return DateTime.Today
            End Get
            Set(ByVal Value As System.DateTime)
                Dim Now As System.DateTime = DateTime.Now
                Dim NewDate As System.DateTime = New DateTime(Value.Year, Value.Month, Value.Day, _
                                                Now.Hour, Now.Minute, Now.Second, Now.Millisecond)
                Dim secondsTimeSpan As System.TimeSpan = NewDate.ToUniversalTime().Subtract(New DateTime(1970, 1, 1, 0, 0, 0))
                Dim seconds As Integer = CType(secondsTimeSpan.TotalSeconds, Integer)

#If TARGET_JVM = False Then
                If (stime(seconds) = -1) Then
                    Throw New UnauthorizedAccessException("The caller is not the super-user.")
                End If
#End If
            End Set
        End Property

        Public ReadOnly Property Timer() As Double
            Get
                Dim DTNow As DateTime = DateTime.Now

                Return DTNow.Hour * 3600 + DTNow.Minute * 60 + _
                        DTNow.Second + DTNow.Millisecond / 1000D
            End Get
        End Property

        Public ReadOnly Property Now() As System.DateTime
            Get
                Return DateTime.Now
            End Get
        End Property

        Public Property TimeOfDay() As System.DateTime
            Get
                Dim TSpan As TimeSpan = DateTime.Now.TimeOfDay

                Return New DateTime(1, 1, 1, TSpan.Hours, _
                 TSpan.Minutes, TSpan.Seconds, _
                 TSpan.Milliseconds)
            End Get
            Set(ByVal Value As System.DateTime)
                Today = DateTime.Now
                Dim NewTime As System.DateTime = New DateTime(Today.Year, Today.Month, Today.Day, _
                                                         Value.Hour, Value.Minute, Value.Second)

                Dim secondsTimeSpan As TimeSpan = NewTime.ToUniversalTime().Subtract(New DateTime(1970, 1, 1, 0, 0, 0, 0))
                Dim seconds As Integer = CType(secondsTimeSpan.TotalSeconds, Integer)

#If TARGET_JVM = False Then
                If (stime(seconds) = -1) Then
                    Throw New UnauthorizedAccessException("The caller is not the super-user.")
                End If
#End If
            End Set
        End Property

        Public Property TimeString() As String
            Get
                Return DateTime.Now.ToString("HH:mm:ss")
            End Get
            Set(ByVal Value As String)
                Dim formats() As String = {"HH:mm:ss", "H:mm:ss", "h:mm:ss", "hh:mm:ss", "H:mm:ss tt", "h:mm:ss t", "hh:mm", "hh:mm tt", "h:mm", "h:mm tt", "h:m", "h:m tt"}

                Try
                    Dim dtToday As DateTime = DateTime.ParseExact(Value, formats, _
                     DateTimeFormatInfo.CurrentInfo, _
                     DateTimeStyles.None)

                    TimeOfDay = dtToday
                Catch e As FormatException
                    Throw New InvalidCastException(String.Format("Cast from string {0} to type '{1}' is not valid.", Value, "Date"))
                End Try
            End Set
        End Property

        ' Methods
        Public Function DateAdd(ByVal Interval As DateInterval, _
        ByVal Number As Double, ByVal DateValue As System.DateTime) As System.DateTime

            Select Case Interval
                Case DateInterval.Year
                    Return DateValue.AddYears(CType(Number, Integer))
                Case DateInterval.Quarter
                    Return DateValue.AddMonths(CType((Number * 3), Integer))
                Case DateInterval.Month
                    Return DateValue.AddMonths(CType(Number, Integer))
                Case DateInterval.WeekOfYear
                    Return DateValue.AddDays(Number * 7)
                Case DateInterval.Day, DateInterval.DayOfYear, DateInterval.Weekday
                    Return DateValue.AddDays(Number)
                Case DateInterval.Hour
                    Return DateValue.AddHours(Number)
                Case DateInterval.Minute
                    Return DateValue.AddMinutes(Number)
                Case DateInterval.Second
                    Return DateValue.AddSeconds(Number)
                Case Else
                    Throw New ArgumentException
            End Select
        End Function

        Public Function GetDayRule(ByVal StartOfWeek As FirstDayOfWeek, ByVal DayRule As DayOfWeek) As DayOfWeek
            Select Case StartOfWeek
                Case FirstDayOfWeek.System
                    Return DayRule
                Case FirstDayOfWeek.Sunday
                    Return DayOfWeek.Sunday
                Case FirstDayOfWeek.Monday
                    Return DayOfWeek.Monday
                Case FirstDayOfWeek.Tuesday
                    Return DayOfWeek.Tuesday
                Case FirstDayOfWeek.Wednesday
                    Return DayOfWeek.Wednesday
                Case FirstDayOfWeek.Thursday
                    Return DayOfWeek.Thursday
                Case FirstDayOfWeek.Friday
                    Return DayOfWeek.Friday
                Case FirstDayOfWeek.Saturday
                    Return DayOfWeek.Saturday
                Case Else
                    Throw New ArgumentException
            End Select
        End Function

        Public Function GetWeekRule(ByVal StartOfYear As FirstWeekOfYear, ByVal WeekRule As CalendarWeekRule) As CalendarWeekRule

            Select Case StartOfYear
                Case FirstWeekOfYear.System
                    Return WeekRule
                Case FirstWeekOfYear.FirstFourDays
                    Return CalendarWeekRule.FirstFourDayWeek
                Case FirstWeekOfYear.FirstFullWeek
                    Return CalendarWeekRule.FirstFullWeek
                Case FirstWeekOfYear.Jan1
                    Return CalendarWeekRule.FirstDay
                Case Else
                    Throw New ArgumentException
            End Select
        End Function

        Public Function DateDiff(ByVal Interval As DateInterval, _
        ByVal Date1 As System.DateTime, ByVal Date2 As System.DateTime, _
        Optional ByVal StartOfWeek As FirstDayOfWeek = FirstDayOfWeek.Sunday, _
        Optional ByVal StartOfYear As FirstWeekOfYear = FirstWeekOfYear.Jan1) As Long

            Dim YearMonths As Integer
            Dim YearQuarters As Integer
            Dim YearWeeks As Integer
            Dim WeekRule As CalendarWeekRule = CalendarWeekRule.FirstDay
            Dim DayRule As DayOfWeek = DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek
            Dim CurCalendar As Calendar = CultureInfo.CurrentCulture.Calendar

            Select Case Interval
                Case DateInterval.Year
                    Return Date2.Year - Date1.Year
                Case DateInterval.Quarter
                    YearQuarters = (Date2.Year - Date1.Year) * 4
                    Return Convert.ToInt64(Date2.Month / 4 - Date1.Month / 4 + YearQuarters)
                Case DateInterval.Month
                    YearMonths = (Date2.Year - Date1.Year) * 12
                    Return Date2.Month - Date1.Month + YearMonths
                Case DateInterval.WeekOfYear
                    YearWeeks = (Date2.Year - Date1.Year) * 53
                    DayRule = GetDayRule(StartOfWeek, DayRule)
                    WeekRule = GetWeekRule(StartOfYear, WeekRule)
                    If (CurCalendar Is Nothing) Then
                        Throw New NotImplementedException("Looks like CultureInfo.CurrentCulture.Calendar is still returning null")
                    End If
                    Return CurCalendar.GetWeekOfYear(Date2, WeekRule, DayRule) - _
                     CurCalendar.GetWeekOfYear(Date1, WeekRule, DayRule) + _
                                       YearWeeks
                Case DateInterval.Weekday
                    Return Convert.ToInt64(((Date2.Subtract(Date1)).Days \ 7))
                Case DateInterval.DayOfYear, _
                     DateInterval.Day
                    Return Date2.Subtract(Date1).Days
                Case DateInterval.Hour
                    Return Convert.ToInt64(Date2.Subtract(Date1).TotalHours)
                Case DateInterval.Minute
                    Return Convert.ToInt64(Date2.Subtract(Date1).TotalMinutes)
                Case DateInterval.Second
                    Return Convert.ToInt64(Date2.Subtract(Date1).TotalSeconds)
                Case Else
                    Throw New ArgumentException
            End Select
        End Function

        Public Function ConvertWeekDay(ByVal Day As DayOfWeek, ByVal Offset As Integer) As Integer
            If (Offset = 0) Then
                Return CType(Day + 1, Integer)
            End If

            Dim Weekday As Integer = CType(Day, Integer) + 1 - Offset
            If (Weekday < 0) Then
                Weekday += 7
            End If

            Return Weekday + 1

            '/*
            '         If (Offset >= 7) Then
            '             Offset -= 7
            '         End If

            '         Dim Weekday As Integer = CType(Day + Offset, Integer)

            '         If (Weekday > 7) Then
            '             Weekday -= 7
            '         End If

            'select (DayOfWeek)Weekday 
            '             Case DayOfWeek.Sunday
            '		return (int)FirstDayOfWeek.Sunday
            '             Case DayOfWeek.Monday
            '		return (int)FirstDayOfWeek.Monday
            '             Case DayOfWeek.Tuesday
            '		return (int)FirstDayOfWeek.Tuesday
            '             Case DayOfWeek.Wednesday
            '		return (int)FirstDayOfWeek.Wednesday
            '             Case DayOfWeek.Thursday
            '		return (int)FirstDayOfWeek.Thursday
            '             Case DayOfWeek.Friday
            '		return (int)FirstDayOfWeek.Friday
            '             Case DayOfWeek.Saturday
            '		return (int)FirstDayOfWeek.Saturday
            '	default:
            '                 Throw New ArgumentException
            '         End Select
            '*/

        End Function

        Public Function DatePart( _
        ByVal Interval As Microsoft.VisualBasic.DateInterval, _
        ByVal DateValue As System.DateTime, _
        Optional ByVal StartOfWeek As FirstDayOfWeek = FirstDayOfWeek.Sunday, _
        Optional ByVal StartOfYear As FirstWeekOfYear = FirstWeekOfYear.Jan1) As Integer

            Dim WeekRule As CalendarWeekRule = CalendarWeekRule.FirstDay
            Dim DayRule As DayOfWeek = DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek
            Dim CurCalendar As Calendar = CultureInfo.CurrentCulture.Calendar

            Select Case Interval
                Case DateInterval.Year
                    Return DateValue.Year
                Case DateInterval.Quarter
                    Return Convert.ToInt32(DateValue.Month / 4 + 1)
                Case DateInterval.Month
                    Return DateValue.Month
                Case DateInterval.WeekOfYear
                    DayRule = GetDayRule(StartOfWeek, DayRule)
                    WeekRule = GetWeekRule(StartOfYear, WeekRule)
                    Return CurCalendar.GetWeekOfYear(DateValue, WeekRule, DayRule)
                Case DateInterval.Weekday
                    Return ConvertWeekDay(DateValue.DayOfWeek, StartOfWeek)
                Case DateInterval.DayOfYear
                    Return DateValue.DayOfYear
                Case DateInterval.Day
                    Return DateValue.Day
                Case DateInterval.Hour
                    Return DateValue.Hour
                Case DateInterval.Minute
                    Return DateValue.Minute
                Case DateInterval.Second
                    Return DateValue.Second
                Case Else
                    Throw New ArgumentException
            End Select
        End Function

        Public Function DateIntervalFromString( _
                        ByVal Interval As String) As DateInterval
            Select Case Interval
                Case "yyyy"
                    Return DateInterval.Year
                Case "q"
                    Return DateInterval.Quarter
                Case "m"
                    Return DateInterval.Month
                Case "ww"
                    Return DateInterval.WeekOfYear
                Case "w"
                    Return DateInterval.Weekday
                Case "d"
                    Return DateInterval.Day
                Case "y"
                    Return DateInterval.DayOfYear
                Case "h"
                    Return DateInterval.Hour
                Case "n"
                    Return DateInterval.Minute
                Case "s"
                    Return DateInterval.Second
                Case Else
                    Throw New ArgumentException("Argument 'Interval' is not a valid value")
            End Select
        End Function

        Public Function DateAdd(ByVal Interval As String, _
        ByVal Number As Double, ByVal DateValue As System.Object) As System.DateTime

            If (DateValue Is Nothing) Then
                Throw New ArgumentNullException("DateValue", "Value can not be null.")
            End If
            If (Not (TypeOf DateValue Is DateTime)) Then
                Throw New InvalidCastException
            End If

            Return DateAdd(DateIntervalFromString(Interval), Number, CType(DateValue, DateTime))
        End Function

        Public Function DateDiff(ByVal Interval As String, _
        ByVal Date1 As System.Object, ByVal Date2 As System.Object, _
        Optional ByVal StartOfWeek As FirstDayOfWeek = FirstDayOfWeek.Sunday, _
        Optional ByVal StartOfYear As FirstWeekOfYear = FirstWeekOfYear.Jan1) As System.Int64

            If (Date1 Is Nothing) Then
                Throw New ArgumentNullException("Date1", "Value can not be null.")
            End If
            If (Date2 Is Nothing) Then
                Throw New ArgumentNullException("Date2", "Value can not be null.")
            End If
            If (Not (TypeOf Date1 Is DateTime)) Then
                Throw New InvalidCastException
            End If
            If (Not (TypeOf Date2 Is DateTime)) Then
                Throw New InvalidCastException
            End If

            Return DateDiff(DateIntervalFromString(Interval), CType(Date1, DateTime), _
                                                CType(Date2, DateTime), StartOfWeek, StartOfYear)

        End Function

        Public Function DatePart(ByVal Interval As String, _
        ByVal DateValue As System.Object, _
        Optional ByVal StartOfWeek As FirstDayOfWeek = FirstDayOfWeek.Sunday, _
        Optional ByVal StartOfYear As FirstWeekOfYear = FirstWeekOfYear.Jan1) As System.Int32

            If (DateValue Is Nothing) Then
                Throw New ArgumentNullException("DateValue", "Value can not be null.")
            End If
            If (Not (TypeOf DateValue Is DateTime)) Then
                Throw New InvalidCastException
            End If


            Return DatePart(DateIntervalFromString(Interval), _
                    CType(DateValue, DateTime), StartOfWeek, StartOfYear)

        End Function

        Public Function DateSerial(ByVal Year As Integer, _
                                        ByVal Month As Integer, _
                                        ByVal Day As Integer) As System.DateTime
            Dim _date As DateTime

            If (Year < 0) Then
                Year = Year + DateTime.Now.Year
            ElseIf (Year >= 0 And Year <= 29) Then
                Year += 2000
            ElseIf (Year >= 30 And Year <= 99) Then
                Year += 1900
            End If

            _date = New DateTime(Year, 1, 1)

            _date = _date.AddMonths(Month - 1)

            _date = _date.AddDays(Day - 1)

            Return _date
        End Function

        Public Function TimeSerial(ByVal Hour As Integer, _
                                        ByVal Minute As Integer, _
                                        ByVal Second As Integer) As System.DateTime
            Dim day As Integer = 1

            If (Second < 0) Then
                If (Minute = 0 And Hour = 0) Then
                    Second += 60
                ElseIf (Minute = 0) Then
                    Second += 60
                    Minute = 59
                    Hour = Hour - 1
                Else
                    Second += 60
                    Minute = Minute - 1
                End If

            ElseIf (Second > 59) Then
                Minute += Convert.ToInt32(Second \ 60)
                Second = Convert.ToInt32(Decimal.Remainder(Second, 60))
            End If

            If (Minute < 0) Then
                If (Hour = 0) Then
                    Minute += 60
                Else
                    Minute += 60
                    Hour = Hour - 1
                End If
            ElseIf (Minute > 59) Then
                Hour += Convert.ToInt32(Minute \ 60)
                Minute = Convert.ToInt32(Decimal.Remainder(Minute, 60))
            End If

            If (Hour < 0) Then
                Hour += 24
            ElseIf (Hour > 23) Then
                day += Convert.ToInt32(Hour \ 24)
                Hour = Convert.ToInt32(Decimal.Remainder(Hour, 24))
            End If

            Return New DateTime(1, 1, day, Hour, Minute, Second)
        End Function

        Public Function DateValue(ByVal StringDate As String) As System.DateTime
            Try
                Dim d As DateTime = DateTime.Parse(StringDate, _
                System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.NoCurrentDateDefault)

                Return d.Subtract(d.TimeOfDay)
            Catch exception As FormatException
                Throw New InvalidCastException(String.Format("Cast from string {0} to type '{1}' is not valid.", StringDate, "Date"))
            End Try
        End Function

        Public Function TimeValue(ByVal StringTime As String) As System.DateTime
            Try
                Return DateTime.MinValue.Add(DateTime.Parse(StringTime).TimeOfDay)
            Catch exception As FormatException
                Throw New InvalidCastException(String.Format("Cast from string {0} to type '{1}' is not valid.", StringTime, "Date"))
            End Try
        End Function

        Public Function Year(ByVal DateValue As System.DateTime) As Integer
            Return DateValue.Year
        End Function

        Public Function Month(ByVal DateValue As System.DateTime) As Integer
            Return DateValue.Month
        End Function

        Public Function Day(ByVal DateValue As System.DateTime) As Integer
            Return DateValue.Day
        End Function

        Public Function Hour(ByVal TimeValue As System.DateTime) As Integer
            Return TimeValue.Hour
        End Function

        Public Function Minute(ByVal TimeValue As System.DateTime) As Integer
            Return TimeValue.Minute
        End Function

        Public Function Second(ByVal TimeValue As System.DateTime) As Integer
            Return TimeValue.Second
        End Function

        Public Function Weekday(ByVal DateValue As System.DateTime, _
        Optional ByVal StartOfWeek As FirstDayOfWeek = FirstDayOfWeek.Sunday) As Integer
            Return DatePart(DateInterval.Weekday, DateValue, StartOfWeek, FirstWeekOfYear.System)
        End Function

        Public Function MonthName(ByVal Month As Integer, _
        Optional ByVal Abbreviate As Boolean = False) As System.String
            'LAMESPEC: MSDN states that in 12-month calendar the 
            '13 month should return empty string, but exception is thrown instead
            If (Month < 1 Or Month > 13) Then
                Throw New ArgumentException("Argument 'Month' is not a valid value.")
            End If
            If (Abbreviate) Then
                Return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Month)
            Else
                Return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month)
            End If
        End Function

        Public Function WeekdayName(ByVal Weekday As Integer, _
        Optional ByVal Abbreviate As System.Boolean = False, _
        Optional ByVal FirstDayOfWeekValue As FirstDayOfWeek = FirstDayOfWeek.System) As System.String

            If (Weekday < 1 Or Weekday > 7) Then
                Throw New ArgumentException("Argument 'Weekday' is not a valid value.")
            End If

            Dim dti As DateTimeFormatInfo = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat
            If (FirstDayOfWeekValue = FirstDayOfWeek.System) Then
                FirstDayOfWeekValue = CType(dti.FirstDayOfWeek, FirstDayOfWeek)
            Else
                FirstDayOfWeekValue = CType((FirstDayOfWeekValue - 1), FirstDayOfWeek)
            End If

            Weekday += FirstDayOfWeekValue - FirstDayOfWeek.Sunday
            Weekday = Convert.ToInt32(Decimal.Remainder(Weekday, 7))
            If (Abbreviate) Then
                Return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedDayName(CType(Weekday, DayOfWeek))
            Else
                Return CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(CType(Weekday, DayOfWeek))
            End If
        End Function
    End Module
End Namespace