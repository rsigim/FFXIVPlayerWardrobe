# FFXIVPlayerWardrobe

Build requirements + Maintenance guide:
* Export the contents of the folders /ui/icon/131000,132000,133000,134000,135000,136000 into \FFXIVPlayerWardrobe\Resources via FFXIV Explorer(06 dat)
* Export the sheets charamakecustomize.exh,enpcbase.exhenpcresident.exh_en,item.exh_en,territorytype.exh,weather.exh_en,weatherrate.exh as CSV into \FFXIVPlayerWardrobe\Resources via FFXIV Explorer(0a dat)
* Export the file chara/xls/human.cmp into \FFXIVPlayerWardrobe\Resources via FFXIV Explorer(04 dat) 
* Adjust player struct offsets in \FFXIVPlayerWardrobe\Memory\Definitions.cs, export as Json via the Json attribute and update the definitions.json file in the repo root

Special thanks to:
* Clorifex
* Liinko
