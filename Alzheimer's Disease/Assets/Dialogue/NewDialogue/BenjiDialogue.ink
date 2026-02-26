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
- 2.1: -> phase2check
// - 2.1: -> phase2GetDishSoap
// - 2.2: -> phase2DishSoapReceived
// - 2.3: -> phase2GetTrashBags
// - 2.4: -> phase2TrashBagsReceived
- 3: -> phase3start
- 3.1: -> phase3GetTape
- 3.2: -> phase3TapeReceived
- 4: -> phase4
}

=== phase1 ===
Hey, kid. #speaker:Anton #portrait:Character_Anton_Neutral
Hey, Pa. #speaker:B?nj? #portrait:Character_Benji_Neutral
-> DONE

=== phase1check ===
{p1SonDone and p1HaveRoller:
    -> phase1RollerReceived
- else:
    -> phase1GetRoller
}

=== phase1GetRoller ===
Hey, kid. #speaker:Anton #portrait:Character_Anton_Neutral
Benji, Pa. #speaker:Benji #portrait:Character_Benji_Sad
... #speaker:Anton #portrait:Character_Anton_Neutral
Sorry.
It's fine. #speaker:Benji #portrait:Character_Benji_Neutral
Don't worry about it.
Did you need something?
Yeah, I was looking for some paints. #speaker:Anton #portrait:Character_Anton_Neutral
Oh. #speaker:Benj? #portrait:Character_Benji_Neutral
I'm not sure where Ma put it. 
Sorry.
That's okay. #speaker:Anton #portrait:Character_Anton_Neutral
I'll keep looking. 
Do you have any paint brushes or something?
She asked for some too.
Uhh... #speaker:Be??i #portrait:Character_Benji_Neutral
I have a paint roller, is that okay?
That works, thanks. #speaker:Anton #portrait:Character_Anton_Neutral
I was gonna bring it with us to the new house for our own redecorating. #speaker:Ben?i #portrait:Character_Benji_Neutral
I'll just get it from her later when we finish packing.
Actually...
Pa, can you help us later with moving everything?
+ ... #speaker:Anton #portrait:Character_Anton_Sad
    Do you really have to go?
    We've talked about this, Pa. #speaker:B?nji #portrait:Character_Benji_Sad
    It'll be easier for me and Liza.
    And with a child on the way,
    We want the best for him too.
    ... #speaker:Anton #portrait:Character_Anton_Sad
    I understand.
    It's just...
+ Sure. #speaker:Anton #portrait:Character_Anton_Neutral
    What do you need help with?
    I think it's just gonna be a lot of boxes. #speaker:B??ji #portrait:Character_Benji_Neutral
    It'd go faster with more people.
    So you'd be a big help. #speaker:B???i #portrait:Character_Benji_Happy
    Alright. #speaker:Anton #portrait:Character_Anton_Neutral
    ... #speaker:Anton #portrait:Character_Anton_Sad
-
We'll miss you. #speaker:Anton #portrait:Character_Anton_Sad
I'll miss you too, Pa. #speaker:Benji #portrait:Character_Benji_Sad
We both will.
But, we'll be sure to visit. #speaker:Benji #portrait:Character_Benji_Happy
You better. #speaker:Anton #portrait:Character_Anton_Neutral
And bring little Anthony with you!
We haven't decided on a name yet, Pa! #speaker:Benji #portrait:Character_Anton_Neutral
And sorry to say, #speaker:Benj? #portrait:Character_Benji_Sad
We're probably not naming him after you.
Well, why not? #speaker:Anton #portrait:Character_Anton_Angry
Anthony is a good name! #speaker:Anton #portrait:Character_Anton_Neutral
Strong. 
Reliable.
Sure, Pa. #speaker:Ben?? #portrait:Character_Benji_Happy
But that's your name.
We want him to have his own.
Hm. #speaker:Anton #portrait:Character_Anton_Neutral
I guess that's good too.
Thanks, Pa. #speaker:Benj? #portrait:Character_Benji_Happy
// ~ p1q1s1done = true // delete after
// ~ p1RollerDone = true
~ p1SonDone = true
// ~ gamePhase = 1.2
-> DONE

=== phase1RollerReceived ===
Hey, Pa. #speaker:Benji #portrait:Character_Benji_Neutral
Need anything else?
Not right now. #speaker:Anton #portrait:Character_Anton_Neutral
Thanks.
-> DONE

=== phase2start ===
... #speaker:Anton #portrait:Character_Anton_Neutral
Hey, uh-
Hm? #speaker:B???? #portrait:Character_Benji_Neutral
Hey, kid. #speaker:Anton #portrait:Character_Anton_Neutral
\(sigh) #speaker:Benji #portrait:Character_Benji_Sad
Benji.
... #speaker:Anton #portrait:Character_Anton_Sad
Yeah. #speaker:Anton #portrait:Character_Anton_Neutral
Benji.
I knew-
Know.
That.
I know that.
Of course I know that.
Mhm. #speaker:Benji #portrait:Character_Benji_Sad
Aren't you supposed to be at school right now. #speaker:Anton #portrait:Character_Anton_Neutral
What? #speaker:Ben?i #portrait:Character_Anton_Neutral
Pa, I graduated 4 years ago.
I work in sales, now. Remember?
Oh. #speaker:Anton #portrait:Character_Anton_Sad
Right.
Sorry.
It's fine. #speaker:Be??i #portrait:Character_Anton_Sad
... #speaker:Anton #portrait:Character_Anton_Sad
Do you need help with the dishes?
I'm good for now. #speaker:B?n?? #portrait:Character_Benji_Sad
Oh, actually... #speaker:Be??i #portrait:Character_Benji_Neutral
We're almost out of dish soap.
Can you get some?
And some trash bags too.
It's filled up.
+ Sure. #speaker:Anton #portrait:Character_Anton_Neutral
    Where'd we put them?
    Check by the front door, maybe. #speaker:Be?j? #portrait:Character_Benji_Neutral
    Not sure if we packed away after groceries.
    Okay. #speaker:Anton #portrait:Character_Anton_Neutral
+ [No.]
    So bossy. #speaker:Anton #portrait:Character_Anton_Neutral
    Why are you making this old man do so much?
    It's just a few items, Pa! #speaker:B??ji #portrait:Character_Benji_Neutral
    You can handle yourself.
    And I still have all the oily pots to wrestle with.
    So, can you please get the stuff?
    Hmp. #speaker:Anton #portrait:Character_Anton_Neutral
    Excuses.
    Fine.
Thank you, Pa. #speaker:B?nj? #portrait:Character_Benji_Neutral
~ gamePhase = 2.1
-
-> DONE

=== phase2check ===
Did you find the stuff yet? #speaker:B???i #portrait:Character_Benji_Neutral
Uh... #speaker:Anton #portrait:Character_Anton_Neutral
\(sigh)  #speaker:B???i #portrait:Character_Benji_Neutral
The dish soap and the trashbags.
Right. #speaker:Anton #portrait:Character_Anton_Neutral
Uh.
+ [Not yet.] #speaker:Anton #portrait:Character_Anton_Neutral
    No, not yet.
    Okay. #speaker:B??j? #portrait:Character_Benji_Sad
    Let me know when you do.
+ [Yes.] #speaker:Anton #portrait:Character_Anton_Neutral
    {p2HaveSoap and p2HaveTrash and p2WifeDone and p2DILDone:
        -> phase2TrashBagsReceived
    - else:
        ...? #speaker:B?n?? #portrait:Character_Benji_Neutral
        Pa... #speaker:B?n?? #portrait:Character_Benji_Sad
        ... #speaker:Anton #portrait:Character_Anton_Neutral
        Sorry.
        It's fine. #speaker:B??ji #portrait:Character_Benji_Sad
        Let me know when you find them. 
    }
-
-> DONE

// p sure we can delete these from here
=== phase2GetDishSoap ===

-> DONE

=== phase2DishSoapReceived ===
Here you go. #speaker:Anton #portrait:Character_Anton_Neutral
Thanks, Pa. #speaker:Be??i #portrait:Character_Benji_Happy
Actually...
While you're here, can you get some trashbags too?
Those should be in the bathroom.
One of the cabinets under the sink.
+ [Why?] #speaker:Anton #portrait:Character_Anton_Neutral
    Why not get it yourself?
    Aren't you done with the dishes?
    \(sigh) #speaker:B?nj? #portrait:Character_Benji_Sad
    I wish.
    The oil is being stubborn.
    The new dish soap should help better. #speaker:Ben?? #portrait:Character_Benji_Neutral
    But until then...
    Trashbags?
    \(sigh) #speaker:Anton #portrait:Character_Anton_Neutral
    Fine.
+ Sure.  #speaker:Anton #portrait:Character_Anton_Neutral
    How do you know they're there?
    From the last time we got groceries. #speaker:Ben?? #portrait:Character_Benji_Neutral
    We put them there, remember?
    You were with me.
    ... #speaker:Anton #portrait:Character_Anton_Neutral
    Right.
    Of course, I remember.
    That was just last week.
    ... #speaker:B???i #portrait:Character_Benji_Sad
    Last month, Pa.
    ... #speaker:Anton #portrait:Character_Anton_Sad
    \(sigh) #speaker:???j? #portrait:Character_Benji_Sad
    It's okay, Pa. #speaker:??nj? #portrait:Character_Benji_Neutral
    You don't need to force yourself.
    Can you just get the trashbags?
    ... #speaker:Anton #portrait:Character_Anton_Sad
    Okay.
- 
Thanks again, Pa. #speaker:Ben?i #portrait:Character_Benji_Happy
// ~ gamePhase = 2.3
-> DONE

=== phase2GetTrashBags ===
I'm missing something. #speaker:Anton #portrait:Character_Anton_Neutral
-> DONE
// to here. i just have commitment issues


=== phase2TrashBagsReceived ===
#test here if enough pieces
{ dilPieces > 6:
    -> recognized
    - else:
    -> notRecognized
        
}
= recognized
TODO: Write recognized scenario (while still forcing DIL out).
Here. #speaker:Anton #portrait:Character_Anton_Neutral
Thanks, Pa.
Are you about done? #speaker:Anton #portrait:Character_Anton_Neutral
Yeah.
-> DONE
~ gamePhase = 3
= notRecognized
What are you doing in my house?! #speaker:Anton #portrait:Character_Anton_Angry
What? #speaker:????? #portrait:Character_Benji_Neutral
Give me that! #speaker:Anton #portrait:Character_Anton_Angry
Wha- #speaker:B???? #portrait:Character_Benji_Angry
Pa! Hey! 
Let go! #speaker:Anton #portrait:Character_Anton_Angry
I'm already in a bad mood.
Don't make me fight you!
Okay! #speaker:B???i #portrait:Character_Benji_Angry
Okay! I'm letting go!
What's going on in here?! #speaker:Rosalyn #portrait:Character_Rosalyn_Angry
I don't know! #speaker:B??j? #portrait:Character_Benji_Angry
He just stormed in, already angry!
Mahal! #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
It's me!
It's me, Rosa.
Your wife.
It's okay.
You're okay.
You're safe, Mahal.
You're with family.
This is your son, Benji, remember?
We live together.
All four of us, together.
You, me, Benji, and Liza.
Our family.
... #speaker:Anton #portrait:Character_Anton_Sad
Our...
Family...
#black out here
~ gamePhase = 3
-> DONE

=== phase3start ===
... #speaker:Anton #portrait:Character_Anton_Neutral
-> DONE

=== phase3check ===
{ p3WifeDone and p3HaveTape: // need to have talked to wife and gotten the 
    -> phase3TapeReceived
  - else:
    -> phase3GetTape    
}

=== phase3GetTape ===
Did you find the tape, Pa? #speaker:B???i #portrait:Character_Benji_Neutral
Not yet. #speaker:Anton #portrait:Character_Anton_Neutral
Let me know when you do. #speaker:Be??i #portrait:Character_Benji_Neutral
-> DONE

=== phase3TapeReceived ===
I have the tape. #speaker:Anton #portrait:Character_Anton_Neutral
Thanks, Pa. #speaker:??nji #portrait:Character_Benji_Neutral
...
Say...
Can you help Liza with the packing?
What. #speaker:Anton #portrait:Character_Anton_Neutral
I know you two are... #speaker:B???? #portrait:Character_Benji_Neutral
At odds...
But it doesn't feel right to leave things as they are.
Especially since we're leaving soon.
It'll be a while before we see each other again.
No. #speaker:Anton #portrait:Character_Anton_Neutral
Please, Pa? #speaker:Be??? #portrait:Character_Benji_Neutral
I'll talk to her too.
Just-
Talk to each other?
Please.
+ [No.]
    I'd rather not. #speaker:Anton #portrait:Character_Anton_Neutral
    Pa... #speaker:B???? #portrait:Character_Benji_Sad
    \(sigh)
    Why do you have to be so stubborn? #speaker:????? #portrait:Character_Benji_Sad
    Fine. #speaker:????? #portrait:Character_Benji_Neutral
    You don't have to talk to her.
    Can you at least help with the boxes?
    ...  #speaker:Anton #portrait:Character_Anton_Neutral
    Fine.
    Thank you. #speaker:????? #portrait:Character_Benji_Happy
+ [Fine.]
    ... #speaker:Anton #portrait:Character_Anton_Neutral
    Fine.
    Thank you. #speaker:Ben?? #portrait:Character_Benji_Happy
-
~ gamePhase = 4
-> DONE

=== phase4start ===
I can handle the things here. #speaker:B???? #portrait:Character_Benji_Neutral
Go find Liza, Pa.
... #speaker:Anton #portrait:Character_Anton_Neutral
-> DONE

=== phase4check ===
{ p4HaveGame:
    -> phase4PuzzleReceived
  - else:
    -> phase4GetPuzzle
}

=== phase4GetPuzzle ===
Hm? #speaker:Be??? #portrait:Character_Benji_Neutral
Uh... #speaker:Anton #portrait:Character_Anton_Neutral
I'm forgetting something.
What is it? #speaker:B?nj? #portrait:Character_Benji_Neutral
It's uh... #speaker:Anton #portrait:Character_Anton_Neutral
It's fine.
I'll remember.
... #speaker:B???? #portrait:Character_Benji_Neutral
-> DONE

=== phase4PuzzleReceived ===
{ sonPieces > 8:
    -> phase4RememberBenji
- else:
    -> phase4ForgotBenji
}
= phase4RememberBenji
I found the puzzle. #speaker:Anton #portrait:Character_Anton_Neutral
Oh!
Where'd you find it?
TODO: Put location  #speaker:Anton #portrait:Character_Anton_Neutral
Thanks for finding it, Pa.
Open it to check if it still has all its pieces. #speaker:Anton #portrait:Character_Anton_Neutral
Right.
...
I'm not counting this.
Wanna piece it together, Pa?
Just like old times.
I'd like that. #speaker:Anton #portrait:Character_Anton_Happy
-> phase4PuzzleEnd

= phase4ForgotBenji
I missed this puzzle. #speaker:Anton #portrait:Character_Anton_Neutral
Oh!
You found it!
I wonder if the pieces are still complete. #speaker:Anton #portrait:Character_Anton_Neutral
...
Want to help me out, kid?
... #speaker:????? #portrait:Character_Benji_Sad
Of course. 
... #speaker:Anton #portrait:Character_Anton_Neutral
... #speaker:????? #portrait:Character_Benji_Sad
This feels... #speaker:Anton #portrait:Character_Anton_Neutral
Nostalgic. #speaker:Anton #portrait:Character_Anton_Happy
You remind me of my son.
We used to piece puzzles just like this when he was a kid.
...
I miss those days.
... #speaker:????? #portrait:Character_Benji_Sad
I miss you too, Pa.
Hm? #speaker:Anton #portrait:Character_Anton_Neutral
Hah! #speaker:Anton #portrait:Character_Anton_Happy
Very similar indeed.
... #speaker:????? #portrait:Character_Benji_Sad
Seems like it's done. #speaker:Anton #portrait:Character_Anton_Neutral
And all the pieces are here.
Thanks, kid.
-> phase4PuzzleEnd

= phase4PuzzleEnd
Oh, you found it. #speaker:??? #portrait:Character_Liza_Neutral
I'll put it in the luggage, now.
// TAKEN FROM LIZA'S FILE
// === phase4PuzzleReceived === 
{ dilPieces > 8:
    -> phase4RememberLiza
- else:
    -> phase4ForgotLiza
}
= phase4RememberLiza
Mm. #speaker:Anton #portrait:Character_Anton_Neutral

TODO: Good end
~gamePhase = 5
-> DONE

= phase4ForgotLiza
What are you- #speaker:Anton #portrait:Character_Anton_Neutral
Hey! #speaker:Anton #portrait:Character_Anton_Angry
This again... #speaker:??? #portrait:Character_Liza_Neutral
Don't touch that! #speaker:Anton #portrait:Character_Anton_Angry
What now?! #speaker:??? #portrait:Character_Liza_Angry
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
... #speaker:Anton #portrait:Character_Anton_Sad
I'm sorry.
~ gamePhase = 5
-> DONE


=== phase4 ===
Hm? #speaker:??? #portrait:Character_Benji_Neutral
It's nothing. #speaker:Anton #portrait:Character_Anton_Neutral
You just...
Remind me of someone.
...? #speaker:??? #portrait:Character_Benji_Neutral
-> DONE