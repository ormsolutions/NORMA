The bitmaps here were used to create the TransactionLogViewer.ImageList.

Bitmap indices in that ImageList correspond to the TransactionLogView.GlyphIndex enum.

The background color for all images is Magenta.

To change a bitmap:
1) Update the desired bitmap here (or create a new one)
2) Open the TransactionLogViewer file in the form designer.
3) Select the ImageList control
4) Set the TransparentColor property to Magenta (or whatever background color you use on a new bitmap)
5) Delete the old image, first noting the index of the image in the Members list.
6) Add your new image. You will see the Magenta color in the Members list.
7) Clear the Name property for the new image (we don't use it and don't need to store it)
8) Move the new image to the index you noted in #5
9) Close the 'Images Collection Editor' dialog
10) Close all editors associated with the form (it is possible to have up to four related editors open). You will get a save prompt when you close the last editor (save).
11) Reopen the form editor, select the image list, and make sure the new image now shows with a transparent background in the Images Collection Editor dialog.
12) Reset the TransparentColor on the ImageList back to 'Transparent' and resave.
13) If you added a new glyph, be sure to update the TransactionLogViewer.GlyphIndex enum to match the new index.
