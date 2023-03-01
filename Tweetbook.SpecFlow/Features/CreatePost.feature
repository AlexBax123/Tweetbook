Feature: CreatePost
	Simple calculator for adding two numbers

@post
Scenario: Create A Post Succesfully
	Given I am logged in
	When I create a post "MyPost1" with the following Tags
	Then the post is created succesfully

@post
Scenario:  Delete A Post Succesfully
	Given I am logged in as Deleter
	Given the post "MyPost3" exists in the system
	When the post is deleted
	Then the post is deleted succesfully

