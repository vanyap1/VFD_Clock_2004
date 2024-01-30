$regfile = "ATtiny5.dat"
$crystal = 8000000
$hwstack = 4
$swstack = 0
$framesize = 4



config portb.2 = output


'CLKPSR.0 = 0
'CLKPSR.1 = 0
'CLKMSR.0 = 0
'CLKMSR.1 = 0
Do


portb.2 = 0
nop
nop
nop

portb.2 = 1

Loop
end