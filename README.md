TELL: Typed Embedded Logic Language
------
This is a simple little Prologish language implemented as an embedded language in C#.
Embedded means that you can make and call TELL code inside your C# code and mix the two somewhat freely.
Being typed, you get compile-time type checking.  And the combination of the two gives you thinks like
limited IntelliSense support.

* Look at EdgeTest in ProofTests for an example of a simple little logic program.
* Look at ReflectionTest in PrimitiveTests for an example of using it to query native C# data