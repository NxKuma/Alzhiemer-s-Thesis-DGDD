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
Hey, kid. #speaker:Anton #portrait:Character_Anton_Neutral
Hey, Pa. #speaker:B?nj? #portrait:Character_Benji_Neutral
-> DONE

=== phase1check ===
{p1q1s1done:
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
~ p1q1s1done = true
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
I think we just bought a few bottles.
Sure. #speaker:Anton #portrait:Character_Anton_Neutral
Where'd we put them?
Check by the front door, maybe. #speaker:Be?j? #portrait:Character_Benji_Neutral
Not sure if we packed away after groceries.
Okay. #speaker:Anton #portrait:Character_Anton_Neutral
~ gamePhase = 2.1
-> DONE

=== phase2GetDishSoap ===
Did you find the dish soap yet? #speaker:B???i #portrait:Character_Benji_Neutral
Dish soap. #speaker:Anton #portrait:Character_Anton_Neutral
Right.
Uh.
No, not yet.
Okay. #speaker:B??j? #portrait:Character_Benji_Sad
Let me know when you do.
-> DONE

=== phase2DishSoapReceived ===
Here's the dish soap. #speaker:Anton #portrait:Character_Anton_Neutral
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
~ gamePhase = 2.3
-> DONE

=== phase2GetTrashBags ===
I'm missing something. #speaker:Anton #portrait:Character_Anton_Neutral
-> DONE

=== phase2TrashBagsReceived ===
#test here if enough pieces
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
Please?
... #speaker:Anton #portrait:Character_Anton_Neutral
Fine.
Thank you. #speaker:Ben?? #portrait:Character_Benji_Happy
~ gamePhase = 4
-> DONE

=== phase4 ===
Hm? #speaker:??? #portrait:Character_Benji_Neutral
It's nothing. #speaker:Anton #portrait:Character_Anton_Neutral
You just...
Remind me of someone.
...? #speaker:??? #portrait:Character_Benji_Neutral
-> DONE