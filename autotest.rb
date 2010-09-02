watch('StateMachine/.*\.cs$') do |match|
	if system('make StateMachine_specs.dll') 
		system 'nunit-console2 bin/StateMachine_specs.dll'
	end
end

watch('BootStrapper/.*\.cs$') do |match|
	if system('make NinjectBootStrapper_specs_no_dep') 
		system 'nunit-console2 bin/NinjectBootStrapper_specs.dll'
	end
end
