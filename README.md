# LightroomUnsupportedFilesCopier
Copies all of the files from a saved Lightroom Import Results text file items containing unsupported or failed files into a directory for processing or correction.

# Usage
After you import photos and videos into Lightroom you might receive a status indicating that some files could not be added because they are unsupported. Save the results to a text file and inspect its contents. It should look similar to this.

```
Duplicate images (7)
    C:\Users\UserName\Downloads\2020-07\IMG_4599.heic
    C:\Users\UserName\Downloads\2020-07\IMG_4596.heic
    C:\Users\UserName\Downloads\2020-07\IMG_4597.heic
    C:\Users\UserName\Downloads\2020-07\IMG_4598.heic
    C:\Users\UserName\Downloads\2020-07\IMG_4614.heic
    C:\Users\UserName\Downloads\2020-07\61678419529__CFC4528F-F9AF-4A24-9712-234BD6806235.jpg
    C:\Users\UserName\Downloads\2020-07\IMG_4620.heic

These files appear to be unsupported or damaged (8)
    C:\Users\UserName\Downloads\2020-07\IMG_4566.mov
    C:\Users\UserName\Downloads\2020-07\IMG_4567.mov
    C:\Users\UserName\Downloads\2020-07\IMG_4568.mov
    C:\Users\UserName\Downloads\2020-07\IMG_4571.MOV
    C:\Users\UserName\Downloads\2020-07\IMG_4577.MOV
    C:\Users\UserName\Downloads\2020-07\IMG_4578.MOV
    C:\Users\UserName\Downloads\2020-07\IMG_4594.mov
    C:\Users\UserName\Downloads\2020-07\IMG_4620.mov
```

You can run the following command using this CLI tool to copy the files above that failed (excluding the duplicates section) into a folder called `2020`

```
lightroomcopy.exe \
    copy \
    -f C:\Users\johna\Downloads\2020-06.txt \
    -o C:\Users\johna\Downloads\2020
```

Once it has been copied into this directory you can run the .MOV files through handbrake, converting them to matroska files with an MP4 container (not MKV). Re-import from the handbrake output directory and the media files will now land in Lightroom.