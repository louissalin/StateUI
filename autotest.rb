watch('.*.cs$') do |match|
	if system('make all') 
		system 'nunit-console2 bin/StateMachine_specs.dll'
		system 'nunit-console2 bin/NinjectBootStrapper_specs.dll'
	end
end
