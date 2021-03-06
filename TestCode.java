public class img2html extends Frame 
{
	static Image im = null;
	static int W = 1, H = 1;

    	public static void main()
    	{
		
		Scanner sc= new Scanner(System.in);
        int strFromScaner = sc.next();
		strFromScaner = strFromScaner + " ";
        System.out.println(strFromScaner);
        int number = sc.nextInt();
		if (number == 1)
			System.out.println(number);
		int otherNumber = sc.nextInt(); //do not use
        sc.close();
		
		String infile,outfile;
		infile="UNSPECIFIED";
		outfile="UNSPECIFIED";
		boolean paramsOK=false,mode=false;
		if (args.length <= 1) {
			usage();
			paramsOK=false;
		} 
		if (args.length == 2) {
			infile = args[0];
			outfile = args[1];
			paramsOK=true;
		}
		if (paramsOK) {
			System.out.println("img2html :");
			System.out.println("  image file  " + infile);
			System.out.println("  data file   " + outfile);
			img2html widow = new img2html();
			System.out.println("img2html : Loading image file");
			if (widow.processFile(infile)==false) {
				System.exit(0);
			} 			
			System.out.println("img2html : Saving data file");
			saveByteData(outfile,mode);
			System.exit(0);
		}
    	}

	public static void saveByteData(String fn,boolean i)
	{
		int[] grabMapXY;
		grabMapXY		= 	new int[(W * H)];
		int pix;
		PixelGrabber pg = new PixelGrabber(im,0,0,W,H,grabMapXY,0,W); 			
		// start grab
		try {
			pg.grabPixels();
		} catch (InterruptedException e) {
			System.out.println("Map grab failed : " + e);
			return;
		}
		if ((pg.status() & ImageObserver.ABORT) != 0) {
			System.out.println("Map grab failed !");
			return;
		}	
		// need to base r on number of valid pixels
		writeToFn(fn,"<HTML>");
		writeToFn(fn,"");
		writeToFn(fn,"<BODY>");
		writeToFn(fn,"");
		writeToFn(fn,"<TABLE border=0 cellspacing=0 cellpadding=0>");
		int red,green,blue,p;
		String r,g,b;
		Color c;
		for (int y =0; y < H; y++) {	
			
			writeToFn(fn,"  <TR>");
			for (int x =0; x < W; x++) {
				pix=grabMapXY[ y * W + x ];
				c = new Color(pix);
				red=c.getRed();
				blue=c.getBlue();
				green=c.getGreen();
				r=Integer.toHexString(red);
				g=Integer.toHexString(green);
				b=Integer.toHexString(blue);
				if (r.length()==1) { r="0"+r; }
				if (g.length()==1) { g="0"+g; }
				if (b.length()==1) { b="0"+b; }
				writeToFn(fn,"    <TD WIDTH=1 HEIGHT=1 BGCOLOR=#" + r + g + b + ">", " " + x + "," + y + " - of - " + W + "," + H);
				writeToFn(fn,"    </TD>");
			}
			writeToFn(fn,"  </TR>");
		}
		writeToFn(fn,"");
		writeToFn(fn,"</TABLE>");
		writeToFn(fn,"");
		writeToFn(fn,"</BODY>");
		writeToFn(fn,"</HTML>");
	}

	public static void writeToFn(String fn,String Dbytes) {
		try {
			System.out.println(Dbytes );
			DataOutputStream dos = new DataOutputStream( new FileOutputStream(fn,true));
			dos.writeBytes(Dbytes); // 
			dos.writeByte(13); // line feed for each line
           		dos.writeByte(10); // line feed for each line 
			// close here
			dos.close();
     		} catch (IOException e) {
    			System.err.println("File error: " + e.getMessage());
        	}
	}

	public boolean processFile(String fn)
	{
		boolean OK=false;
		int exitcounter;
		try {
			im = getToolkit().getImage(fn);
		}
		catch (Exception e) 
		{
			System.out.println("1HOLY MOLY THIS NEVER HAPPENS!!!! "+e);
		} 
		MediaTracker tracker= new MediaTracker(this);
		tracker.addImage(im,0);
		try {
			tracker.waitForID(0);
		} 
		catch (InterruptedException e) 
		{
			System.out.println("2HOLY MOLY THIS NEVER HAPPENS "+e);
		} 
		if (tracker.isErrorID(0)) {
			System.out.println("IMAGE FILE COULD NOT BE LOADED");
			System.out.println(fn);
		} else {
			exitcounter=0;
			while ( ((W==-1)||(H==-1)) && (exitcounter<=16000000) ) {
				W=im.getWidth(null);
				H=im.getHeight(null);
				exitcounter++;
			}
			if (exitcounter>15999999) {
				System.out.println("Timeout attempting to load image, is it to big?");
			} else {
				OK=true;
			}
		}
		return(OK);
	}
	


	public static void usage() {
		System.out.println("Usage:");
		System.out.println("java img2html infile outfile ");
		System.out.println(" infile  : image file .gif ");
		System.out.println(" outfile : html file to be created");
		//System.out.println(" /i      : Inverse data ");
	}


}	// end of class
