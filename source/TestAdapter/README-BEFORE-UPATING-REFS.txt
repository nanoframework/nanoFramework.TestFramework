********************************************
***                WARNING               ***
*** Do NOT update ICSharpCode.Decompiler ***
*** beyond 7.2.1.6856 as it will break   ***
*** VS Test Explorer                     ***
********************************************

This happens because of a nested dependency on 
System.Runtime.CompilerServices.Unsafe.

Moving beyond this version requires testing this.

Reported and it's a know issue with vstest. See:
https://github.com/microsoft/vstest/issues/4673
https://github.com/microsoft/vstest/issues/4775
