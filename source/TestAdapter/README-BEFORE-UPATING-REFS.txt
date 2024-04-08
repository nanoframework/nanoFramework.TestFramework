*******************************************
***                 WARNING            ****
*** Do NOT update CLIWrap beyond v3.6.1 ***
*** as it will break VS Test Explorer   ***
*******************************************

This happens because of a nested dependency on System.Runtime.CompilerServices.Unsafe.
Moving beyond this version requires testing this.
Reported and it's a know issue wit vstest. See:
https://github.com/microsoft/vstest/issues/4673
https://github.com/microsoft/vstest/issues/4775
