ShiftPlanning C# SDK
================

The [ShiftPlanning API](http://www.shiftplanning.com/api/) allows you to call modules within the ShiftPlanning [employee scheduling software](http://www.shiftplanning.com/) that respond in REST style JSON & XML.

This repository contains the open source Javascript SDK that allows you to utilize the above on your website. Except as otherwise noted, the ShiftPlanning Javascript SDK is licensed under the Apache Licence, Version 2.0 http://www.apache.org/licenses/LICENSE-2.0.html)


Usage
-----

The [examples][examples] are a good place to start. The minimal you'll need to
have is:

	 //initialize SDK where 'XXXXXXXXXXXXXXXXXX' = API Key
         shiftPlanning = new ShiftPlanning("XXXXXXXXXXXXXXXXXX");


Logged in vs Logged out:

	if (shiftPlanning.getLoginStatus())
	{
		//LOGGED IN
		
	}
	else
	{
		//LOGGED OUT
		
	}
	

To make [API][API] calls:

	// call to update wages of employee 101 

	//preparing list of request fields 
	RequestFields employee_data = new RequestFields();
	employee_data.Add("id", 101);
	employee_data.Add("wage", 25);

	response = shiftPlanning.updateEmployee(employee_data);
	if (response.Status.Code == "1")
	{
		// Success
	}



Feedback
--------

We are relying on the [GitHub issues tracker][issues] linked from above for
feedback. File bugs or other issues [here][issues].

[issues]: http://github.com/shiftplanning/cs-sdk/issues