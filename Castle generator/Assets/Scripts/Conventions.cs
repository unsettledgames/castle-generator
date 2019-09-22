/*
 * NAMING CONVENTIONS FOR THE TILES:
 * 
 * The structure of the name of a tile must respect the following format:
 * 
 * [PartName][PartVariationNumber][VerticalAlignment][HorizontalAlignment][Light/Dark][VariationNumber]
 * 
 * - Part name is the name of the structure the tile is part of (e.g. "Bastion", "Wall")
 * - PartVariationNumber is a number assigned from 0 to +infinity added in case of similar structures (e.g. "Bastion0", "Bastion1")
 * - Vertical alignment is the vertical position of the tile relatively to the other ones (e.g. "Top", "Middle", "Bottom")
 * - Horizontal alignment is the horizontal position of the tile relatively to the other ones (e.g. "Left", "Middle", "Right")
 * - Light / Dark tells whether the tile is in the dark part of the structure or in the light part
 *      - Transition tiles, in this case, are labelled as "LightDark", if the tile is light and is used to connect to a dark
 *        one, or "DarkLight" if the tile is dark and is used to connect to a light one
 * - VariationNumber is used in case two tiles serve the same function (e.g. Bastion0MiddleRightDark and Bastion0MiddleRightDark1)
 * 
 * There are elements that could not respect this format: towers, arcs, statues etc, they must be managed in a different way.
 * Each time such an element is added, it is important to add a proper implementation of it in the algorithm.
 * 
 */ 