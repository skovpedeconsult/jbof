# jbof
Don't deploy your CMS. Use JBOF.

Content management systems is something that can make a developer lose their good mood. From a developers point of view there are numerous problems. 

  1. They are complex. 
  2. They are slow unless aggresive caching is used which adds further complexity.
  3. They can be hacked.
  4. They require complex auth systems where editors and users live in the same database.
  5. They are not horizontally scalabale without adding further complexity
  6. They are usually not cloud ready and require their own box for hosting.
  7. They can be really slow to program because web servers and debuggers can be really slow to load.


If we take a step back and look what it says on the box it says content management system. They manage content really well. They are the preferred tool for marketing people with good reason. They are popular for a reason and we fully acknowledge that. 

What it doesn't say on the box is horizontally scalable global deployment. They don't say simple programming model. They don't say fast and secure.

Can we improve on the situation? Introducing JBOF. Just a bunch of files. The JBOF contept is based on a CMS. It doesn't matter which one. The contept works for all of them. When cms's require ever increasing cache layers to function properly, why not just use the ultimate cache layer and copy all the content to individual files. We get the best of both worlds. Content is managed in a system specialized for this task. When the content is ready to be deployed we just deduplicate it out to cheap disks on a web server which can just read the file from disk in the correct language and server it over the wire 95% ready.

But what about the last 5%? I would like to make some of the content restricted. This is your lucky day. Web programming environments like ASP.NET MVC are really good at this. Microsoft are using a bunch of money for making this a plesant experiance for developers with auto scaffolding entire web projects with login for twitter, github etc. Type safe configuration(OWIN). Quick start up times. Self hosting. Easy deploy for cloud environments. Hot swap of code in some cases. Here I am using a MS platform as example, but the point still stand with any platform.

To support restricted content we simply need to match a class or id in thehtml  document with a property in our user database. We could just parse the html with a html parser, AngleSharp, HTML agility pack or whichever your prefer, look for a class on a menu item to decide if it needs to be hidden. As an example we could enrich links to super exclusive partner area with a class: only-show-to-super-partners and hide the links for everybode else. On the actual super partner html files we could put a class somewhere with restrict-to-super-partners which we could scan for and throw a 401 if they somehow guessed the URL.

This project consists of two parts:
  1. A POC of scraping a cms/blog in this case a wordpress blog but it could be any cms/blog. 
  2. A web app which reads files based on a route and adds the current user to a pre determined field on the page.

