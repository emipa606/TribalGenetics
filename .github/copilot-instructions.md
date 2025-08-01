# GitHub Copilot Instructions

## Mod Overview and Purpose
**Mod Name:** Tribal Genetics (Continued)
  
Tribal Genetics is a mod that enhances the interaction with Vanilla Genetics Expanded mechanics, specifically tailored for tribal societies. It introduces new systems to create genetic implants without relying heavily on the existing ancient labs scattered across the game. The mod envisions a unique storyline where tribes, descended from ancient geneticists, mythologize their methods and incorporate them into their cultural rituals and constructs. This allows players to explore a narrative around tribal genetics, offering a fresh take on tribal progression through scientific heritage.

## Key Features and Systems
- **Sacred Genetics Meme:** Unlocks all mod content, introducing new roles and rituals. It is incompatible with Body Purist and Transhumanist.
  
- **Gene Shaman Role:** Enables the extraction of Tier 1 genomes from corpses without genome extractors. Higher-tier genomes still require scavenging.

- **New Rituals:** 
  - **Animal and Prisoner Grinder Sacrifices:** Variations of standard sacrifice rituals leading to different quality genoframes based on ritual outcomes.
  - **Discover Ancient Genetics Lab Outcome:** Allows Ideoligions with Sacred Genetics to find new labs innovatively.

- **Sanctified Genetics Table:** A workstation for crafting implants using a micro-bio-battery powered by meat, eliminating research requirements. Compatible with VGE - More Lab Stuff for genetrainer crafting.

## Coding Patterns and Conventions
- Standard C# naming conventions are used, with classes named in PascalCase and methods in camelCase.
- Public classes and methods are clearly delineated with comments explaining distinct functionalities.
- The code follows a modular structure, allowing for easy updates and maintenance.

## XML Integration
XML is used extensively for defining game objects, traits, jobs, and rituals. The integration ensures that new features and functionalities adhere to RimWorld’s existing schema and are easily extendable.

- Utilize XML for defining new memes, roles, rituals, and workstations.
- Ensure compatibility by adhering to existing Vanilla Genetics Expanded structures.

## Harmony Patching
Harmony is used for patching RimWorld’s existing methods to inject new functionality seamlessly:
- **Efficient Patching:** Only target specific methods that require modifications using Harmony patches to introduce custom behaviors.
- **Conflict Avoidance:** Patch only when necessary to maintain compatibility with other mods.

## Suggestions for Copilot
- When suggesting code for new features, Copilot should highlight usage examples of existing classes such as `CompAbilityExtractGenes`.
- For new patches or modifications, Copilot should propose using Harmony along with examples where applicable.
- When extending or altering XML files, Copilot should recommend attribute conventions and structured data for easy integration.
- Propose descriptive comments and documentation blocks within the codebase to ensure maintainability and clarity for future updates.

This document aims to provide contributors an insight into the mod's structure and facilitate seamless contributions and enhancements. For further details on any particular system, refer to the respective class or method files in the codebase.
