-> intro

=== intro ===
#scene:living_room
 #loop_sfx:ring
 #wait:2
* [\* Zvedni telefon\*] -> pickup1

=== pickup1 ===
#stop_sfx:ring
#wait:0.5
#sfx:hang
#wait:1

Čau XXX, jak jde život? Dlouho jsem o tobě neslyšel. #speaker:scientist

No nic moc, včera měla Kája narozeniny a hrozně se těšila na trampolínu. #speaker:player
No ale znáš naše sousedy, ty by se z toho randálu zcvokli, 
tak jsem tu trampolínu postavil do garáže.
Žejo. 
No a teď mám na krku naštvaný rodiče a policii.
Žejo.
...
Tyjo, Lucka by mohla vědět, jak z toho

Kdo? #speaker:scientist

No Lucka z gymplu, ta co šla na práv- #speaker:player

Se ptal! #speaker:scientist

* [\*Zavěs\*]
#wait:0.5
#sfx:hang 
    
* [Tys na tý trampolíně skákal taky, ne?]
#wait:1
#sfx:hang

- -> intro2


=== intro2 ===
#clear
#wait:2
#loop_sfx:ring
#wait:2
* [\* Zvedni telefon\*] -> pickup2

=== pickup2 ===
#stop_sfx:ring
#wait:0.5
#sfx:hang
#wait:1
Dobrý, promiň, jsem si nemohl pomoct. #speaker:scientist
Samozřejmě mě to mrzí.
Lucka je kapacita, to dáte dohromady.
No ale potřebuji s něčím pomoct, proto volám.

* [...] -> invite

=== invite ===
Hele, víš co? Ja bych o tom neměl mluvit přes telefon, přijeď do XXX #speaker:scientist 
#wait:1
#sfx:hang
#clear
#wait:0,5
-> lab_entry

=== lab_entry ===
#scene:lab
… #speaker:player
…
Co to je..? 

Šnek. Říkáme mu Jeremy II. #speaker:scientist

*[Svítí?]
*[Turbo?] 
*[Umí francouzsky?] 
- -> no_snail

=== no_snail ===
Ne, ty mám v jiný laborce. #speaker:scientist
Tenhle šnek je o hodně vzácnější.

Nepřišlo ti někdy divný, že každý šnek, co potkáš, má pravotočivou ulitu?

 *[No to teda. Jsem rád, že se tomu konečně začli vědci věnovat a neztrácejí čas třeba léčbou rakoviny...]
    To děláme o víkendu. #speaker:scientist
    -> snail
 *[Chceš se zeptat ještě jednou?] 
 Ne. #speaker:scientist
 -> snail
 *[No.. Kája o tom jednou blebtala, ale moc jsem ji neposlouchal]
 Bez komentáře. #speaker:scientist
 -> snail
 
 === snail ===
 #wait:0.5
 No, není pravda, že každý šnek je pravotočivý. Existují i levotočiví šneci, právě jako náš Jeremy II. Taky se jim říká sinistrální. Docela cool, ne?
Může za to špatná exprese Lsdia1 genu, což se stane asi u jednoho z 50 000 šneků.
Je to vlastně takový plží ekvivalent situs inversus, což je to, když má člověk zrcadlově orgány v těle.
Doufáme, že Jeremy II by nám pomohl v porozumění této anomálie.

Dobře, hrozně zajímavý, ale k čemu přesně jsem tu já? Já mám srdce na správném místě. #speaker:player
Chápeš
:p

Jo... #speaker:scientist
Problém je, že Jeremy II se prostě nemá dobře poslední dobou a nevíme, co za to může, ani co s tím.

*[Zkoušeli jste ketamin?]
*[Zkoušeli jste 30mg zolpidemu?]
*[Střelte mu Krokodýla, to jste zkoušeli?]
- -> jeremyI

=== jeremyI===
Jo, zkoušeli.
RIP Jeremy I.
Tentokrát to nechceme nechat náhodě a rozhodli jsme se, že vyšleme někoho přímo do terénu.
Od toho jseš tu ty.
Máme tady tohle zmenšovací zařízení, který jsme vynalezli.
Ještě to má nějaké mouchy, ale není čas to odladit.

Počkej, to vů- #speaker:player
 #clear
- -> terrarium

=== terrarium ===
#scene:blackout
#wait:2
#scene:terrarium
#wait:1
... #speaker:scientist
Je ti něco?
Je mu hodně. #speaker:scientist2
#wait:0.5
Kámo, ty jseš fakt mrdka z krtka. #speaker:player
-> DONE
