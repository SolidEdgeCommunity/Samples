Credit: Martin Bernhard

You have to distinguish between three cases, if you need to get the bodies in your part file:
 
1) You created a single extrusion with multiple disjoint and closed profile, e.g. 3 non-overlapping circles resulting in 3 cylinder bodies
=> You don't have direct access to each body. You need to iterate through all Models and get its body. This body has the IsSolid property set and has multiple closed, non-void Shells.
 
2) You created each feature in a multi-body design by using the AddBody command
=> Each body is stored in its own Model object, just retrieve its Body property
 
3) You created a solid construction body, by using the Extruded surface command with closed ends.
=> Each of these bodies are stored in the Body property of the ConstructionModel object, which can be found in the Constructions collection of the part document