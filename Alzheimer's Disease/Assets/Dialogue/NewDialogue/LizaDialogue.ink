INCLUDE globals.ink

{ gamePhase:
- 0: //tutorial
- 1: -> phase1
- 1.1: -> phase1check
- 2: -> phase2start
- 2.1: -> phase2check
- 3: -> phase3start
- 3.1: -> phase3check
- 4: -> phase4start
- 4.1: -> phase4check
}

=== phase1 ===
Hello, po, Tito. #speaker:Liz? #portrait:Character_Liza_Neutral
...Hi. #speaker:Anton #portrait:Character_Anton_Neutral
-> DONE

=== phase1check ===
{p1DILDone:
    -> phase1BrushReceived
- else:
    -> phase1GetBrush
}

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
~ p1DILDone = true
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

=== phase2check ===
{ p2DILDone:
    -> phase2TrashBagsReceived    
  - else:
    -> phase2GetTrashBags
}


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
~ p2DILDone = true
-> DONE

=== phase2TrashBagsReceived ===
Good riddance. #speaker:Anton #portrait:Character_Anton_Neutral
-> DONE

=== phase3start ===
Why are there so many boxes here? #speaker:Anton #portrait:Character_Anton_Neutral
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
I think it's in one of the balikbayan boxes over
... #speaker:Anton #portrait:Character_Anton_Neutral
Fine.
Thanks, Pa. #speaker:Be??i #portrait:Character_Benji_Neutral
~ gamePhase = 3.1
-> DONE

=== phase3check ===
Hmp. #speaker:Anton #portrait:Character_Anton_Neutral
-> DONE

=== phase4start ===
... #speaker:Anton #portrait:Character_Anton_Neutral
I'm here for the boxes. 
... #speaker:L??? #portrait:Character_Liza_Neutral
Here.
Can you help with these?
Hm. #speaker:Anton #portrait:Character_Anton_Neutral
... 
... #speaker:L??a #portrait:Character_Liza_Neutral
I...
I've been meaning to ask.
There was something Benji wanted to bring with us.
An old puzzle set?
A puzzle set? #speaker:Anton #portrait:Character_Anton_Neutral
What would he want with that?
He wants to play with it when our child arrives. #speaker:Li?? #portrait:Character_Liza_Neutral
Fatherly bond or something.
Hm. #speaker:Anton #portrait:Character_Anton_Neutral
Like my son and I used to do...
Yeah. #speaker:L?z? #portrait:Character_Liza_Neutral
He mentioned that.
...
\(sigh)
This is a long shot.
But do you know where you kept it?
... #speaker:Anton #portrait:Character_Anton_Neutral
I think I do.
Great. #speaker:L??? #portrait:Character_Liza_Neutral
If you do, show it to Benji.
He'll be happy you found it.
Okay. #speaker:Anton #portrait:Character_Anton_Neutral
~ gamePhase = 4.1
-> DONE

=== phase4check ===
{ p4HaveGame:
    -> phase4PuzzleReceived
- else:
    -> phase4GetPuzzle
}
-> DONE

=== phase4GetPuzzle ===
Have you found it? #speaker:L??? #portrait:Character_Liza_Neutral
-> DONE

=== phase4PuzzleReceived ===
Huh. #speaker:L??? #portrait:Character_Liza_Neutral
I'm surprised you found it.
That's good.
Go show Benji.
I'm sure he'll be happy to see it.
Mm. #speaker:Anton #portrait:Character_Anton_Neutral
-> DONE