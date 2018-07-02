////////////////////////////////////////////////////////1
jQuery(document).ready(function(){

jQuery("#slider").slider({min: 0,max: 100,values: [25,63],range: true,
	stop: function(event, ui) {jQuery("input#minCost").val(jQuery("#slider").slider("values",0));jQuery("input#maxCost").val(jQuery("#slider").slider("values",1));},
    slide: function(event, ui){jQuery("input#minCost").val(jQuery("#slider").slider("values",0));jQuery("input#maxCost").val(jQuery("#slider").slider("values",1));}
});

jQuery("input#minCost").change(function(){var value1=jQuery("input#minCost").val();var value2=jQuery("input#maxCost").val();
    if(parseInt(value1) > parseInt(value2)){value1 = value2;jQuery("input#minCost").val(value1);}
	jQuery("#slider").slider("values",0,value1);
});

jQuery("input#maxCost").change(function(){var value1=jQuery("input#minCost").val();var value2=jQuery("input#maxCost").val();
	if (value2 > 1000) { value2 = 1000; jQuery("input#maxCost").val(1000)}
	if(parseInt(value1) > parseInt(value2)){value2 = value1;jQuery("input#maxCost").val(value2);}
	jQuery("#slider").slider("values",1,value2);
});

/////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////2

jQuery("#slider2").slider({min: 0,max: 150000,values: [0,150000],range: true,
	stop: function(event, ui) {jQuery("input#minCost2").val(jQuery("#slider2").slider("values",0));jQuery("input#maxCost2").val(jQuery("#slider2").slider("values",1));},
    slide: function(event, ui){jQuery("input#minCost2").val(jQuery("#slider2").slider("values",0));jQuery("input#maxCost2").val(jQuery("#slider2").slider("values",1));}
});

jQuery("input#minCost2").change(function(){var value1=jQuery("input#minCost2").val();var value2=jQuery("input#maxCost2").val();
    if(parseInt(value1) > parseInt(value2)){value1 = value2;jQuery("input#minCost2").val(value1);}
	jQuery("#slider2").slider("values",0,value1);	
});	

jQuery("input#maxCost2").change(function(){var value1=jQuery("input#minCost2").val();var value2=jQuery("input#maxCost2").val();
	if (value2 > 1000) { value2 = 1000; jQuery("input#maxCost2").val(1000)}
	if(parseInt(value1) > parseInt(value2)){value2 = value1;jQuery("input#maxCost2").val(value2);}
	jQuery("#slider2").slider("values",1,value2);
});

//////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////3

jQuery("#slider3").slider({min: 0,max: 365,values: [0,365],range: true,
	stop: function(event, ui) {jQuery("input#minCost3").val(jQuery("#slider3").slider("values",0));jQuery("input#maxCost3").val(jQuery("#slider3").slider("values",1));},
    slide: function(event, ui){jQuery("input#minCost3").val(jQuery("#slider3").slider("values",0));jQuery("input#maxCost3").val(jQuery("#slider3").slider("values",1));}
});

jQuery("input#minCost3").change(function(){var value1=jQuery("input#minCost3").val();var value2=jQuery("input#maxCost3").val();
    if(parseInt(value1) > parseInt(value2)){value1 = value2;jQuery("input#minCost3").val(value1);}
	jQuery("#slider3").slider("values",0,value1);	
});	

jQuery("input#maxCost3").change(function(){var value1=jQuery("input#minCost3").val();var value2=jQuery("input#maxCost3").val();
	if (value2 > 1000) { value2 = 1000; jQuery("input#maxCost3").val(1000)}
	if(parseInt(value1) > parseInt(value2)){value2 = value1;jQuery("input#maxCost3").val(value2);}
	jQuery("#slider3").slider("values",1,value2);
});

//////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////4

jQuery("#slider4").slider({min: 0,max: 20000,values: [0,20000],range: true,
	stop: function(event, ui) {jQuery("input#minCost4").val(jQuery("#slider4").slider("values",0));jQuery("input#maxCost4").val(jQuery("#slider4").slider("values",1));},
    slide: function(event, ui){jQuery("input#minCost4").val(jQuery("#slider4").slider("values",0));jQuery("input#maxCost4").val(jQuery("#slider4").slider("values",1));}
});

jQuery("input#minCost4").change(function(){var value1=jQuery("input#minCost4").val();var value2=jQuery("input#maxCost4").val();
    if(parseInt(value1) > parseInt(value2)){value1 = value2;jQuery("input#minCost4").val(value1);}
	jQuery("#slider4").slider("values",0,value1);	
});	

jQuery("input#maxCost4").change(function(){var value1=jQuery("input#minCost4").val();var value2=jQuery("input#maxCost4").val();
	if (value2 > 1000) { value2 = 1000; jQuery("input#maxCost4").val(1000)}
	if(parseInt(value1) > parseInt(value2)){value2 = value1;jQuery("input#maxCost4").val(value2);}
	jQuery("#slider4").slider("values",1,value2);
});

//////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////5

jQuery("#slider5").slider({min: 0,max: 100,values: [0,1000],range: true,
	stop: function(event, ui) {jQuery("input#minCost5").val(jQuery("#slider5").slider("values",0));jQuery("input#maxCost5").val(jQuery("#slider5").slider("values",1));},
    slide: function(event, ui){jQuery("input#minCost5").val(jQuery("#slider5").slider("values",0));jQuery("input#maxCost5").val(jQuery("#slider5").slider("values",1));}
});

jQuery("input#minCost5").change(function(){var value1=jQuery("input#minCost5").val();var value2=jQuery("input#maxCost5").val();
    if(parseInt(value1) > parseInt(value2)){value1 = value2;jQuery("input#minCost5").val(value1);}
	jQuery("#slider5").slider("values",0,value1);	
});	

jQuery("input#maxCost5").change(function(){var value1=jQuery("input#minCost5").val();var value2=jQuery("input#maxCost5").val();
	if (value2 > 1000) { value2 = 1000; jQuery("input#maxCost5").val(1000)}
	if(parseInt(value1) > parseInt(value2)){value2 = value1;jQuery("input#maxCost5").val(value2);}
	jQuery("#slider5").slider("values",1,value2);
});

//////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////6

jQuery("#slider6").slider({min: 0,max: 10000,values: [0,10000],range: true,
	stop: function(event, ui) {jQuery("input#minCost6").val(jQuery("#slider6").slider("values",0));jQuery("input#maxCost6").val(jQuery("#slider6").slider("values",1));},
    slide: function(event, ui){jQuery("input#minCost6").val(jQuery("#slider6").slider("values",0));jQuery("input#maxCost6").val(jQuery("#slider6").slider("values",1));}
});

jQuery("input#minCost6").change(function(){var value1=jQuery("input#minCost6").val();var value2=jQuery("input#maxCost6").val();
    if(parseInt(value1) > parseInt(value2)){value1 = value2;jQuery("input#minCost6").val(value1);}
	jQuery("#slider6").slider("values",0,value1);	
});	

jQuery("input#maxCost6").change(function(){var value1=jQuery("input#minCost6").val();var value2=jQuery("input#maxCost6").val();
	if (value2 > 1000) { value2 = 1000; jQuery("input#maxCost6").val(1000)}
	if(parseInt(value1) > parseInt(value2)){value2 = value1;jQuery("input#maxCost6").val(value2);}
	jQuery("#slider6").slider("values",1,value2);
});
});
//////////////////////////////////////////////////////////