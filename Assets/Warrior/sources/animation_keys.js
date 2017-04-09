    var teclado: AnimationClip;
    
    function Update () {
    
    if (Input.GetKey("w")){
    	animation.Play ("run");		
    }
    		
    else if(Input.GetKey("m"))
    		{
  			  animation.Play ("attack");}
  			  
  	else if(Input.GetKey("3"))
    		{
  			  animation.Play ("walk");}		  

    else if(Input.GetKey("4"))
    		{
  			  animation.Play ("jump");}
  			  
  	else 
   			 {
    		animation.Play ("idle");}

   	 }
    



    
    
    
    
    
