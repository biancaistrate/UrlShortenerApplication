Feature: Manage tiny urls in the system

	Scenario: Tiny urls get created successfully
		When I create tinyUrls with the following details
			| OriginalUrl      | Alias      |
			| https://www.toolsqa.com/specflow/tables-in-specflow/ | specflow |
			| https://www.pragimtech.com/blog/blazor/rest-api-repository-pattern/ | repo   |
		Then tinyUrls are created successfully
			| OriginalUrl      | Alias      |
			| https://www.toolsqa.com/specflow/tables-in-specflow/ | specflow |
			| https://www.pragimtech.com/blog/blazor/rest-api-repository-pattern/ | repo   |

	Scenario: TinyUrl gets retrieved successfully
		Given a tinyurl with alias specflow is created in the system
		When I request a TinyUrl by alias specflow
		Then If I access the tiny url with alias specflow I get redirected to my orignal url