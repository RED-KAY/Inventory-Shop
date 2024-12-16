# Introduction 
- This Unity C# project demonstrates how applying well-established software design patterns—specifically MVC, the Observer pattern, and the Service Locator—improves code organization and reduces coupling. Using a simple inventory and shop scenario, it illustrates disciplined architectural approaches that make projects more maintainable, scalable, and easier to understand.

# Architecture
1. Core Systems
    - The project comprises three main systems:

    1. Shop System
    2. Inventory System
    3. Random Drop System
    (Additionally, a player economy mechanism supports buying and selling items.)

2. Inter-System Communication
    - While each system is designed to be modular, they still need to interact to achieve the desired functionality.

3. Communication Mechanisms
    - Interactions between systems are facilitated through the Observer Pattern and a GameController Service, ensuring a decoupled yet cohesive architecture.

4. MVC Implementation
    - The Shop and Inventory systems follow the Model-View-Controller (MVC) pattern. You’ll find similar architectural approaches applied to other parts of the project as well.

5. UML Diagram
    - A detailed UML diagram illustrating the overall architecture is provided [here](https://app.diagrams.net/#G1Ys3kpttWdCPPyVSl0xiRSEvcZRAN9I1R#%7B%22pageId%22%3A%2276Zj12aouIvC6FJQ6CF7%22%7D) to help you visualize the system’s design.

# Features
1. Core Functionality
    - All the fundamental shop and inventory capabilities are fully implemented. These include buying, selling, and managing items.

2. Random Drop System
    - This system generates five random items based on their rarity. Rarer items have a lower probability of appearing and are available in limited quantities, while more common items have a higher probability and drop in greater numbers.

3. Quality of Life Improvements

    1. Automatic Space Management: Items that cannot fit into the inventory due to limited space are automatically disabled, preventing clutter and confusion.
    2. Intelligent Item Placement: Newly acquired items from random drops are sorted by rarity first, then by weight, ensuring optimal organization.
    3. Filtering System: Users can filter items by type, making it easier to find and manage specific categories of items.

Note: The current visual presentation prioritizes functionality over aesthetics. Future updates will focus on refining the UI/UX once the core architectural goals have been demonstrated.

# Preview
https://github.com/user-attachments/assets/4ead4438-4bbb-4dd3-a86b-584a081acef6

