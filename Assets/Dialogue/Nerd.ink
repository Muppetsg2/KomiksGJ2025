-> main

=== main ===
I am a nerd if you want to know!!!! #speaker:Nerd #portrait:nerd_talk #layout:right
HAHAHAHAHAHAH!!!!

But I want to know... which game was first?
+ [Mario] -> gameChosen("Mario")
+ [Donkey Kong] -> gameChosen("DonkeyKong")
+ [Człowiek Małpa] -> gameChosen("ClowekMalpa")

=== gameChosen(game) ===
{game == "DonkeyKong":
    You are right!!! Congratulations!!! -> END
- else:
    You are wrong!!!
    -> badChoice("Good choice was Donkey Kong")
}

=== badChoice(msg) ===
{msg}

-> END