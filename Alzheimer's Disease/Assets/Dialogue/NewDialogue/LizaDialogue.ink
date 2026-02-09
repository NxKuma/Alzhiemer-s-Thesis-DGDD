INCLUDE globals.ink

{ gamePhase:
- 0: //tutorial
- 0.1: //
- 1: -> phase1
- 1.1: -> phase1check
// - 1.1: -> phase1GetRoller
// - 1.2: -> phase1RollerReceived
// - 1.3: -> phase1GetBrush
// - 1.4: -> phase1BrushReceived
- 2: -> phase2start
- 2.1: -> phase2GetDishSoap
- 2.2: -> phase2DishSoapReceived
- 2.3: -> phase2GetTrashBags
- 2.4: -> phase2TrashBagsReceived
- 3: -> phase3start
- 3.1: -> phase3GetTape
- 3.2: -> phase3TapeReceived
- 4: -> phase4
}

=== phase1 ===
Hello, po, Tito. #speaker:Liz? #portrait:Character_Liza_Neutral
...Hi. #speaker:Anton #portrait:Character_Anton_Neutral
-> DONE

=== phase1check ===
{p1q1d1done:
    -> phase1BrushReceived
- else:
    -> phase1GetBrush
}

=== phase1GetRoller ===
Who is this person? #speaker:Anton #portrait:Character_Anton_Neutral
...
Focus, Anton.
-> DONE

=== phase1RollerReceived ===
I was supposed to do something else... #speaker:Anton #portrait:Character_Anton_Neutral
-> DONE

=== phase1GetBrush ===
Who are you? #speaker:Anton #portrait:Character_Anton_Neutral
Huh? #speaker:??? #portrait:Character_Liza_Neutral
I said, who are you?! #speaker:Anton #portrait:Character_Anton_Angry
\(sigh) #speaker:Liza #portrait:Character_Liza_Neutral
I'm Liza, po, Tito. 
Your son's wife.
Liza? #speaker:Anton #portrait:Character_Anton_Neutral
...
Right.
Liza.
What are you doing here?
I was just washing my brushes. #speaker:Liza #portrait:Character_Liza_Neutral
... #speaker:Anton #portrait:Character_Anton_Neutral
Those aren't yours.
...What? #speaker:Li?a #portrait:Character_Liza_Neutral
Those aren't your brushes! #speaker:Anton #portrait:Character_Anton_Angry
Those are Rosa's!
Give them back!
What?! #speaker:L??? #portrait:Character_Liza_Angry
No!
What do you mean, "No"? #speaker:Anton #portrait:Character_Anton_Angry
How dare you!
She gave them to me! #speaker:L??a #portrait:Character_Liza_Angry
You were there when she did!
I- #speaker:Anton #portrait:Character_Anton_Angry
What? #speaker:Anton #portrait:Character_Anton_Neutral
Why would she do that?
... #speaker:L?za #portrait:Character_Liza_Angry
You told her to. #speaker:L??a #portrait:Character_Liza_Neutral
You learned that I wanted to try painting.
So you suggested that she lend me her old brushes.
... #speaker:Anton #portrait:Character_Anton_Neutral
I did?
Yes. #speaker:Liz? #portrait:Character_Liza_Neutral
You did.
I- #speaker:Anton #portrait:Character_Anton_Neutral
I'm sorry...
\(huff) #speaker:L?za #portrait:Character_Liza_Neutral
It's fine.
I'm used to it.
...
... #speaker:Anton #portrait:Character_Anton_Neutral
Was there something you needed? #speaker:Liz? #portrait:Character_Liza_Neutral
I uh- #speaker:Anton #portrait:Character_Anton_Neutral
I think I did need those brushes.
Rosa needs it.
Here. #speaker:L?z? #portrait:Character_Liza_Neutral
... #speaker:Anton #portrait:Character_Anton_Neutral
Thank you.
Mhm. #speaker:Li?? #portrait:Character_Liza_Neutral
~ p1q1d1done = true
// ~ gamePhase = 1.4
-> DONE

=== phase1BrushReceived ===
Did you need something else? #speaker:Li?a #portrait:Character_Liza_Neutral
Not right now. #speaker:Anton #portrait:Character_Anton_Neutral
Mm. #speaker:L?za #portrait:Character_Liza_Neutral
-> DONE

=== phase2start ===
Someone's here. #speaker:Anton #portrait:Character_Anton_Neutral
...
I should leave.
-> DONE

=== phase2GetDishSoap ===
I shouldn't be here. #speaker:Anton #portrait:Character_Anton_Neutral
-> DONE

=== phase2DishSoapReceived ===
Who is this person? #speaker:Anton #portrait:Character_Anton_Neutral
...
I should get back to the task first.
-> DONE

=== phase2GetTrashBags ===
Excuse me. #speaker:Anton #portrait:Character_Anton_Neutral
Huh? #speaker:???a #portrait:Character_Liza_Neutral
Excuse me. #speaker:Anton #portrait:Character_Anton_Neutral
I need to get to the cabinet behind you, Miss.
Oh. #speaker:?i?? #portrait:Character_Liza_Neutral
What did you need?
I'll get it for you.
I just need the trashbags. #speaker:Anton #portrait:Character_Anton_Neutral
I can do it myself.
Please move.
It's fine. #speaker:L??? #portrait:Character_Liza_Neutral
I', already here.
... #speaker:Anton #portrait:Character_Anton_Neutral
Who do you think you are?! #speaker:Anton #portrait:Character_Anton_Neutral
\(sigh) Again? #speaker:L??a #portrait:Character_Liza_Neutral
I'm Liza.
Your daughter-in-law.
Daughter? #speaker:Anton #portrait:Character_Anton_Neutral
I don't have a daughter.
Daughter-IN-LAW. #speaker:L??? #portrait:Character_Liza_Angry
Your son's wife?
Wife? #speaker:Anton #portrait:Character_Anton_Neutral
My son is married?
When did he-
About a year ago, now. #speaker:L?z? #portrait:Character_Liza_Neutral
And I've been staying here for more than that.
Why can't you ever seem to remember me?
You can't even remember my name! #speaker:L?z? #portrait:Character_Liza_Angry
It's Liza, by the way. #speaker:Liza #portrait:Character_Liza_Neutral
L-I-Z-A.
Liza.
It's not even that hard of a name.
... #speaker:Anton #portrait:Character_Anton_Neutral
Excuse you, Liza.
Are you this rude to everyone you talk to?
Especially in someone else's house?
Anyone else would have kicked you out by now.
Don't worry. #speaker:Liz? #portrait:Character_Liza_Neutral
I don't plan on staying.
We're moving out by the end of the week.
You won't have to see me again.
Just take the trashbags.
Now, can you please leave me alone?
The sooner you do, the sooner we're out of each others' hair.
Fine. #speaker:Anton #portrait:Character_Anton_Neutral
~ gamePhase = 2.4
-> DONE

=== phase2TrashBagsReceived ===
Good riddance. #speaker:Anton #portrait:Character_Anton_Neutral
-> DONE

=== phase3start ===
Why are there so many boxes? #speaker:Anton #portrait:Character_Anton_Neutral
We're moving out. #speaker:Li?? #portrait:Character_Liza_Neutral
Remember?
Like you wanted us to?
Liza, don't be like that. #speaker:Be??i #portrait:Character_Benji_Sad
Sorry, Pa.
She's just...
Heated.
Hmp. #speaker:L?za #portrait:Character_Liza_Neutral
Hm. #speaker:Anton #portrait:Character_Anton_Neutral
\(sigh) #speaker:B?nj? #portrait:Character_Benji_Sad
You said you'd help us with the packing, Pa. #speaker:B??ji #portrait:Character_Benji_Neutral
Can you find some packing tape?
... #speaker:Anton #portrait:Character_Anton_Neutral
Fine.
Thanks, Pa. #speaker:Be??i #portrait:Character_Benji_Neutral
~ gamePhase = 3.1
-> DONE

=== phase3GetTape ===
Hmp. #speaker:Anton #portrait:Character_Anton_Neutral
-> DONE

=== phase3TapeReceived ===
Hm. #speaker:Anton #portrait:Character_Anton_Neutral
-> DONE

=== phase4 ===
Is that...? #speaker:Anton #portrait:Character_Anton_Neutral
Hey! #speaker:Anton #portrait:Character_Anton_Angry
This again... #speaker:??? #portrait:Character_Liza_Neutral
Don't touch that! #speaker:Anton #portrait:Character_Anton_Angry
What is it this time?! #speaker:??? #portrait:Character_Liza_Angry
Let go of that puzzle box! #speaker:Anton #portrait:Character_Anton_Angry
+ [Take it back.]
+ [Take it back.]
+ [Take it back.]
+ [Take it back.]
-
What are you doing?! #speaker:??? #portrait:Character_Liza_Angry
Let go! #speaker:Anton #portrait:Character_Anton_Angry
This is my son's treasure!
You are not taking it from me!
From us!
+ [Take. It. Back.]
+ [Take. It. Back.]
+ [Take. It. Back.]
+ [Take. It. Back.]
-
This is the last thing for the move! #speaker:??? #portrait:Character_Liza_Angry
We're literally done after this!
Stop being so stubborn!
And let.
Us.
Go!
+ [Protect your family.]
-
AH! #speaker:??? #portrait:Character_Liza_Angry
What's going on in here?! #speaker:??? #portrait:Character_Benji_Neutral
Liza?! #speaker:??? #portrait:Character_Benji_Angry
Pa, what did you do?! 
...What? #speaker:Anton #portrait:Character_Anton_Sad
What happened? #speaker:Rosaly? #portrait:Character_Rosalyn_Neutral
Oh my god. #speaker:Rosal?? #portrait:Character_Rosalyn_Sad
Anton, what did you do?
I- #speaker:Anton #portrait:Character_Anton_Sad
I didn't-
She was-
Oh, thank god. #speaker:??? #portrait:Character_Benji_Sad
She's okay.
...
Maybe it's a good thing we're moving after all.
... #speaker:Rosa??? #portrait:Character_Rosalyn_Sad
... #speaker:Anton #portrait:Character_Anton_Sad
...You should get a caregiver, Ma. #speaker:??? #portrait:Character_Benji_Sad
I don't want this to happen to you too. #speaker:??? #portrait:Character_Benji_Neutral
I would never- #speaker:Anton #portrait:Character_Anton_Angry
You just did, Pa. #speaker:B???? #portrait:Character_Benji_Neutral
You just did. #speaker:B???? #portrait:Character_Benji_Sad
...
The truck's coming in an hour. #speaker:B???? #portrait:Character_Benji_Neutral
Stay away from Liza for the time being.
... #speaker:Anton #portrait:Character_Anton_Neutral
I'm sorry.
~ gamePhase = 5
-> DONE