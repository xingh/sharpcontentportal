#This the archive of the comment that have been put on the project home page.

# Project Home Archive #
---
02.24.2008:  Thing have been moving slow, but they are moving forward.  Real life has been getting in the way of finishing the work flow enhancements that I want to put in at the page level.  My goal was to integrate the new content module as a part of the core but I will be extracting it as a stand alone piece.  Still looking for anyone that wants to help, contact me if you are interested.

12.22.2007:  I want to take a moment a wish everyone well as this year comes to a close and for those that celebrate one of the many events this time of the year a happy holiday.  I hope that the new year will see a new time of peace and prosperity for all.  I will not be shy in saying that I look forward to the end of my countries occupation of Iraq.

I have been busy and have not been able to spend time working on the software, but I now have a few day off of work and hope to complete the updates that I have been working on.  If every thing goes well I want publish the new version by January 1st, 2008.


11.26.2007:  It has been a while since I posted any thing new, so here is a little update.  I am close to finishing the first crack at the enhanced content management features.  This will include two new roles, an author and a publisher role.  These new built in roles will allow for administrators to delegate authoring and publishing functionality without handing over the "key to the castle" so to speak.  I hope to have this update wrapped up and on the site before the end of the year.

The project has gained two new members and I hope that more will join, so I want to extended an invitation to anyone who would like to join the project and help to keep it moving forward.  If you have interest in the project, please email me at thomas.jason.t@gmail.com.

For those that have downloaded the first working version, if you have discovered any bugs please report them on the Issues tab so they can be resolved, thanks.  Also, any feed back about the software is also welcome.


10.19.2007:  Release 1.1.1 is available for download.


10.19.2007:  We have a working copy!  It builds and it runs.  Now on to new features.


10.16.2007:  The updates just keep coming.  It runs but there are still bugs.  Not ready for prime time, but close.


10.10.2007:  It builds and it almost runs!  I loads the home page, but it still needs a little work.  There are a few bugs hiding in the code, such as you can not login or register.  I had reworked part of the login process adding the idea of predefined password reset questions, but I did that under the Oracle provider.  I have not yet implemented all of the hooks for it under the MS SQL provider.  Work continues...


09.30.2007:  I have checked the code in to the trunk.  It builds but I have not run it yet, so no promises that it works.


09.23.2007:  Hello all and welcome to my first open source project that I am the host/owner.  Let me start with a little back ground, I have been a fan of the DotNetNuke framework for the past two year and when I was given the a project to upgrade my companies corporate website and customer support site from classic asp to ASP.NET, I decided to use DotNetNuke as the the foundation for the new sites.  As I discussed the solution with my management they had concerns about implementing a web site written in Visual Basic due to the small fact that all the staff developers, with the exception of myself, were Java developers.  So I searched the web for a portal solution that was open source and written in C# and .Net 2.0.  I almost found what I was looking for with the Rainbow portal (www.rainbowportal.net), but it ran with .Net 1.1 at the time.  I finally found what I was looking for here at Google Code with the cs-dotnetnuke project (http://code.google.com/p/cs-dotnetnuke).  It was a C# port of DotNetNuke but still needed to be debugged, so I started debugging.  Several months later I had finished debugging as well as the addition of some custom enhancements requested by my management, one of which being a the ability to run the software with Oracle. Java and Oracle, what a pair.  I provided many of the fixes that I made to the cs-dotnetnuke project but as time went on the custom enhancements I had made took the software away from the baseline with which I had started.  I did not want to force those enhancements on the cs-dotnetnuke project but I felt that I needed to offer the code back to the community that had provided it to me.  So, here it is, renamed and still needing a little work.

So what still needs to be done, a lot.  Because of the work that I did to built the Oracle provider combined with the custom enhancements the SQL Provider is now broken and needs to be fixed.  I will need to remove some of the enhancements as they are of little use to anyone but my company; I did not want to but them in to start with but they insisted.  The renaming of many of the classes has also created some problems that will need to be resolved.  After the software functions again using MS SQL as the database, I want to move the HTML/Text module into the software as a core feature and expand its functionality to include a simple content publishing work flow with two new roles, author and publisher, that provide checks and balances to who can modify content and who can publish that content to the world.

Although it is not perfect, I hope that others can find some use for this code.  Also want to thank everyone that has contributed to get the code to were it is now.  So, lets get start and write some code.

JT - thomas.jason.t@gmail.com