# PreStack
Organizes FITS files in preparation for PixInsight image processing.

PreStack is a utility for assembling image files into a directory structure which is optimized for PixInsight image processing.  
For each set of target images (by directory), the user can select a set of flat images (by directory), a Bias master and a Darks master.  
The program will segregate light images by position angle (for rotation) and associate corresponding flats.  
Image files are moved into newly organized directories.  Flats, Bias and Darks images are copied so they may be reassembled with subsequent targets.

Documentation can be found in the publish directory:  PreStack Description.pdf

