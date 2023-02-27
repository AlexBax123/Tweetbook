Feature: CreatePost
	Simple calculator for adding two numbers

@mytag
Scenario: Create A Post Succesfully
	Given I am logged in
	When I create a post with the following Tags
	Then the post is created succesfully

Scenario:  Delete A Post Succesfully
	Given I am logged in
	Given the post exists in the system
		| Name    |
		| MyPost1 |
	When the post is deleted
	Then the post is deleted succesfully