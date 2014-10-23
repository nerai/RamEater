RamEater
========

Trivial program which keeps allocating chunks of RAM for testing system behavior.

Allocation will occur in chunks starting at 1GB size. The number of chunks is unlimited. In case an OutOfMemory exception is thrown, the size will be reduced. Each chunk will filled with data to ensure its memory pages were actually created and initialized.

In particular, running on a x64 box, you should be able to observe that .NET is able page out to disk nicely after filling up available physical RAM.

Please note that this will very likely slow down your system severly, possibly crashing other running applications in the process (web browsers are notorious for this).
