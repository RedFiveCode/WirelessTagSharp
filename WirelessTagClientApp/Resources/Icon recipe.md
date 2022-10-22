# Icon recipe


## Preparation
1. Download FontAwesome temperature-high svg icon from https://fontawesome.com/icons/temperature-high?s=solid&f=classic ; save the file as `temperature-high-solid.svg`.
1. Open as xml file in VSCode.
1. Pretty format the xml.
1. Add `fill="#ffffff"` to the `<path ... />` element. Set fill colour to white or desired background colour; defaults to black if the `fill` attribute is absent.
1. Save file with different name, for example `temperature-high-solid-white.svg`.

## Icon editing
1. Open Axialis IconWorkshop (from https://www.axialis.com/iconworkshop/).
1. Create new Windows icon.
1. Size 128 pixels (to match the background, see next step).
1. Open Gradient background (`~\Documents\Axialis Librarian\Objects\Pack 5 - Web Illustrations\Background Bases\Gradient Border\Gradient Border Blue Rounded.png`).
1. Drag onto icon surface.
1. In the Librarian, navigate to folder containing `temperature-high-solid-white.svg`. 
1. Select `temperature-high-solid-white.svg` and drag onto the background image.
1. Resize and move the svg image to fit within the gradient border.
1. Edit icon as desired.
1. Create smaller icon sizes from original icon as needed.
1. Save icon file.
1. Add to the Visual Studio project as a resource.

(end)