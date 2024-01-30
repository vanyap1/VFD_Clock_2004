$regfile = "m8def.dat"
$crystal = 16000000
$hwstack = 40
$swstack = 16
$framesize = 32
$baud = 38400


Dim S As String * 255 , Ns As Byte , Rs As Byte , Len_str As Byte
Config Serialin = Buffered , Size = 255
Declare Sub Getline(s_dan As String)
Declare Sub Flushbuf()
Flushbuf

rst Alias Portb.1 : Config rst = Output : reset rst
Ce1 Alias Portb.0 : Config Ce1 = Output                       'Display Enable
Ce2 Alias Portd.7 : Config Ce2 = Output                       'Display Enable
Mclk Alias Portd.6 : Config Mclk = Output : set mclk                  'SPI CLOK
Dat Alias Portd.5 : Config Dat = Output                     'SPI MOSI


Declare Sub W_data(byval W_bytes As Byte)
Dim S1 As String * 16                                      '700
Dim S2 As String * 16
Dim Buf1(16) As Byte At S1 Overlay
Dim Buf2(16) As Byte At S2 Overlay

Dim second_tick As Long

Config Clock = User                                         'Use USER to write/use your own code
Config Date = Dmy , Separator = .                           'Day.Month.Year


Config Scl = Portd.2
Config Sda = Portd.3
Config I2cdelay = 1
I2cinit
Const Pcf21w = &HA2 : Const Pcf21r = &HA3

Declare Sub Read_time(h_pcf As Byte , M_pcf As Byte , S_pcf As Byte )
Declare Sub Read_date(date_pcf As Byte , Month_pcf As Byte , Year_pcf As Byte)
Declare Sub Write_time(byval H_pcf As Byte , Byval M_pcf As Byte , Byval S_pcf As Byte )
Declare Sub Write_date(byval Date_pcf As Byte , Byval Month_pcf As Byte , Byval Year_pcf As Byte )


'Date$ = "01.09.09"
'Time$ = "22:17:00"

Config Timer1 = Timer , Prescale = 256
On Timer1 Timer_irq
Const Timer_preload = 3035                                'Timervorgabe fur Sekunden Takt

Enable Timer1
Enable Interrupts


set rst

dim seco as byte
dim dimm as byte
dim bright as byte
bright = 7

'call Write_time( 19 , 39 , 00)
'call Write_date( 25 , 10 , 19)
s2 = " __ Its Work __"
do




If Ischarwaiting() = 1 Then
Waitms 10
Getline S
Flushbuf
if mid(s , 1, 4) = "$tim" then
s1 = mid(s , 5 , 8)
Time$ = mid(s , 5 , 8)
call Write_time(_hour , _min , _sec )
end if

if mid(s , 1, 4) = "$lim" then
s2 = mid(s , 5 , 16)
end if
end if


s1 = "    " + Time$
's1 = "Consumption -"

do
if len(s1) < 16 then
s1= s1 + " "
end if
loop until len(s1) => 16
do
if len(s2) < 16 then
s2= s2 + " "
end if
loop until len(s2) => 16


Reset Ce1
waitus 20
dimm = bright + 8
call W_data(248)
call W_data(7)
call W_data(dimm)   'dimmer
call W_data(246)
call W_data(245)
call W_data(224)
call W_data(Buf1(16))
call W_data(Buf1(15))
call W_data(Buf1(14))
call W_data(Buf1(13))
call W_data(Buf1(12))
call W_data(Buf1(11))
call W_data(Buf1(10))
call W_data(Buf1(9))
call W_data(Buf1(8))
call W_data(Buf1(7))
call W_data(Buf1(6))
call W_data(Buf1(5))
call W_data(Buf1(4))
call W_data(Buf1(3))
call W_data(Buf1(2))
call W_data(Buf1(1))
call W_data(244)
call W_data(241)
Set Ce1


Reset Ce2
waitus 20
dimm = bright + 8
call W_data(248)
call W_data(7)
call W_data(dimm)   'dimmer
call W_data(246)
call W_data(245)
call W_data(224)
call W_data(Buf2(16))
call W_data(Buf2(15))
call W_data(Buf2(14))
call W_data(Buf2(13))
call W_data(Buf2(12))
call W_data(Buf2(11))
call W_data(Buf2(10))
call W_data(Buf2(9))
call W_data(Buf2(8))
call W_data(Buf2(7))
call W_data(Buf2(6))
call W_data(Buf2(5))
call W_data(Buf2(4))
call W_data(Buf2(3))
call W_data(Buf2(2))
call W_data(Buf2(1))
call W_data(244)
call W_data(241)
Set Ce2

waitms 100

loop






Sub W_data(byval W_bytes As Byte,  Sspi As Byt) As Byte
Dim Sspi As Byte

For Sspi = 1 To 8
Dat = W_bytes.7
reSet Mclk
Shift W_bytes , Left , 1
set Mclk
Next
waitus 10
End Sub


Timer_irq:                                                 'Timer1 IRQ (once per second)
Call Read_time(_hour , _min , _sec )
Call Read_date(_day , _month , _year )
'print Time$
'Time$ =  Time(second_tick)
Timer1 = Timer_preload

                                              'only for Bascom-Simulator
Return

Settime:
Return

Getdatetime:
Return

Setdate:
Return

Sub Getline(s As String)
S = ""
Ns = Inkey()
If Ns <> 10 Then
$timeout = 100
Input S Noecho
If Ns <> 10 Then S = Chr(ns) + S
End If
End Sub

Sub Flushbuf()
Waitms 10
Do
Ns = Inkey()
Loop Until Ns = 0
End Sub



Sub Write_date(date_pcf As Byte , Month_pcf As Byte , Year_pcf As Byte )
Local Weekdays_pcf As Byte
Date_pcf = Makebcd(date_pcf) : Month_pcf = Makebcd(month_pcf)
Year_pcf = Makebcd(year_pcf) : Weekdays_pcf = Makebcd(weekdays_pcf)
I2cstart
I2cwbyte Pcf21w
I2cwbyte &H06
I2cwbyte Date_pcf
I2cwbyte Weekdays_pcf
I2cwbyte Month_pcf
I2cwbyte Year_pcf
I2cstop
End Sub


Sub Read_date(date_pcf As Byte , Month_pcf As Byte , Year_pcf As Byte )
Local Weekdays_pcf As Byte
I2cstart
I2cwbyte Pcf21w
I2cwbyte &H05
I2cstart
I2cwbyte Pcf21r
I2crbyte Date_pcf , Ack                                     '0-31
I2crbyte Weekdays_pcf , Ack                                 '0-6
I2crbyte Month_pcf , Ack                                    '1-12
I2crbyte Year_pcf , nack                                     '0-99
I2cstop
Date_pcf = Makedec(date_pcf) : Weekdays_pcf = Makedec(weekdays_pcf)
Month_pcf = Makedec(month_pcf) : Year_pcf = Makedec(year_pcf)
End Sub


Sub Read_time(h_pcf As Byte , M_pcf As Byte , S_pcf As Byte )
I2cstart
I2cwbyte Pcf21w
I2cwbyte &H02
I2cstart
I2cwbyte Pcf21r
I2crbyte S_pcf , Ack                                        '0-59
I2crbyte M_pcf , Ack                                        '0-59
I2crbyte H_pcf , nack                                        '0-23
I2cstop
If Seco.7 = 1 Then
Reset Seco.7

I2cstart
I2cwbyte Pcf21w
I2cwbyte &H03
I2cwbyte Seco
I2cstop
End If
S_pcf = Makedec(s_pcf) : M_pcf = Makedec(m_pcf) : H_pcf = Makedec(h_pcf)
End Sub

Sub Write_time(h_pcf As Byte , M_pcf As Byte , S_pcf As Byte)
S_pcf = Makebcd(s_pcf) : M_pcf = Makebcd(m_pcf) : H_pcf = Makebcd(h_pcf)
I2cstart
I2cwbyte Pcf21w
I2cwbyte &H03
I2cwbyte S_pcf
I2cwbyte M_pcf
I2cwbyte H_pcf
I2cstop
End Sub

end