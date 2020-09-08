# Check Vitals

This program detects when any of the body vitals go out of limits.
The purpose is to treat without delay.

## Issues

- The code here has high complexity in a single function.
- The tests are not complete - they do not cover all conditions

## Tasks

1. Reduce the cyclomatic complexity.
1. Complete the tests - cover all conditions.
1. To treat, we need to know the abnormal vital and the breach -
whether high or low. Add this capability.
1. Add the ability to plug-in different reporters to this code.
