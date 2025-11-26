INCLUDE globals.ink

{ gamePhase:
- 0: //tutorial
- 0.1: //
- 1: -> phase1
- 1.1: -> phase1check
- 2: //phase2
- 3: //phase3
}

=== phase1 ===
Hello, po, Tito. #speaker:Liz? #portrait:liza
...Hi. #speaker:Anton #portrait:anton
-> DONE

=== phase1check ===
{p1q1d1done:
    -> phase1BrushReceived
- else:
    -> phase1GetBrush
}

=== phase1GetBrush ===
Who are you? #speaker:Anton #portrait:anton
Huh? #speaker:??? #portrait:liza
Who are you?! #speaker:Anton #portrait:anton
And what are you doing in my house?!
\(sigh) #speaker:Liza #portrait:liza
I'm Liza, po, Tito. 
Your son's wife.
Liza? #speaker:Anton #portrait:anton
...
Right.
Liza.
What are you doing here?
I was just washing my brushes. #speaker:Liza #portrait:liza
... #speaker:Anton #portrait:anton
Those aren't yours.
...What? #speaker:Li?a #portrait:liza
Those aren't your brushes! #speaker:Anton #portrait:anton
Those are Rosa's!
Give them back!
What?! #speaker:L??? #portrait:liza
No!
What do you mean, "No"? #speaker:Anton #portrait:anton
How dare you!
She gave them to me, remember?! #speaker:L??a #portrait:liza
You were there when she did!
You're the one that told her to!
I- #speaker:Anton #portrait:anton
What?
Why would she do that?
Because you learned that I wanted to try painting! #speaker:L?za #portrait:liza
So you suggested that she lend me her old brushes.
I... #speaker:Anton #portrait:anton
I did?
Yes. #speaker:Liz? #portrait:liza
You did.
I... #speaker:Anton #portrait:anton
I'm sorry...
\(huff) #speaker:L?za #portrait:liza
It's fine, Tito.
I'm used to it.
...
... #speaker:Anton #portrait:anton
Was there something you needed? #speaker:Liz? #portrait:liza
I uh- #speaker:Anton #portrait:anton
I think I did need those brushes.
Rosa needs it.
Here. #speaker:L?z? #portrait:liza
... #speaker:Anton #portrait:anton
Thank you.
Mhm. #speaker:Li?? #portrait:liza
~ p1q1d1done = true
-> DONE

=== phase1BrushReceived ===
Did you need anything else? #speaker:Li?a #portrait:liza
Not right now. #speaker:Anton #portrait:anton
Mm. #speaker:L?za #portrait:liza
-> DONE