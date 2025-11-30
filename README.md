


# Challenges Encountered / Trials and Tribulations
## Unit Testing
Despite multiple attempts, I was unable to get unit testing
working as intended. The tests either complained about not being able to access
certain members due to protection levels or, in the words of AI, attempted to
“initialize Windows-specific components which are not available in the test
context.” I’m not aware of an easy way to resolve this—or at least not one I’d
feel confident implementing—so the only “testing” I could reliably perform was
manual eyeball testing by running the application (which I know behaves as
expected).

## CI Actions
Given that unit testing wasn’t
possible, I assumed that testing wouldn’t occur in the CI actions either. But
even setting the unit tests aside, I still ran into persistent issues that
prevented the CI workflow from completing successfully. My plan was for the
workflow to check out the repository, set up the .NET 9 SDK, install the MAUI
workloads, restore dependencies, and then build the Windows target before
finishing.

Unfortunately, in all my
attempts, the workflow only made it as far as building the Windows target
before failing with an error about being unable to install the runtime pack for
Microsoft.NETCore.App.Runtime.win-x64, which ultimately caused the build to
fail. At this point, I found myself running in circles, hitting every other
error imaginable but never progressing past this failure—resulting in what is
effectively an impasse.


