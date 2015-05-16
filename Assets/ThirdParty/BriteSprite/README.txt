BriteSprite Instructions:

Before running BriteSprite, make sure that you have at least two lightmaps for your art (one lit from the top, and one lit from the right). These should be grayscale images that are shaded as if your art was being lit by a light directly above or directly to the right of the picture.


(1) Ensure the BriteSprite folder is in your Unity project. If it is, there should be a "BriteSprite" option in the Unity Window menu. (Window->BriteSprite)

(2) The BriteSprite window has five slots for lightmaps, each lit from a different direction. Only the "top" and "right" lightmaps are required, and they will be sufficient for most objects. If you have a complicated piece of art, or just want to get a little bit more fidelity, you can use all five lightmaps. 

If you follow a simple naming convention, BriteSprite can automatically fill in the remaining slots for you. For example, if you put an image in the "Right Lit" slot whose filename ends in "_right", BriteSprite will look in its parent directory for images ending in "_top", "_bottom", "_left", and "_front" and automatically fill in the remaining slots.

(3) Once your lightmaps are slotted in, select your art style (either pixel art or high-resolution art) to ensure the best import settings.

(4) Press the Process button! BriteSprite will generate a high-quality normal map based on your lighting data.


To quickly test your normal maps, create a Quad from GameObject->Create Other->Quad. Then, attach a material to it (using the built-in Transparent Bumped Diffuse shader) and put your normal map in the relevant slot. Add a point light to your scene, and move it around to test the effect!



Questions? Concerns? Need more information? 

Visit our website at www.2dAssassins.com/BriteSprite